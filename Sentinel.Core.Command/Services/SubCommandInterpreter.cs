using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Sentinel.Core.Command.Attributes;
using Sentinel.Core.Command.Enums;
using Sentinel.Core.Command.Interfaces;
using Sentinel.Core.Command.Models;
using Sentinel.Core.Helpers;

namespace Sentinel.Core.Command.Services
{
    public class SubCommandInterpreter<T>
    {
        private Dictionary<string, Type> commandCache;
        private readonly IServiceProvider serviceProvider;

        public SubCommandInterpreter(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;

            commandCache = new Dictionary<string, Type>();

            var types = typeof(T).GetNestedTypes();
            foreach (var type in types)
            {
                var attribute = type.GetCustomAttribute(typeof(SubCommandAttribute));
                if (attribute != null)
                {
                    var subCommandAttribute = (SubCommandAttribute) attribute;
                    commandCache.Add(subCommandAttribute.BaseCommand, type);
                }
            }
        }

        public int Execute(IShell shell, string[] commandLine, TextReader input, TextWriter output, TextWriter error)
        {
            if (commandLine.Length == 0)
            {
                //TODO do something here
                return 1;
            }
            var command = commandLine[0];
            var args = commandLine.Skip(1).ToArray();

            var commands = GetCommands(command, shell);

            switch (commands.Count)
            {
                case 0:
                    Console.WriteLine($"Command Not Found: \"{command}\"");
                    break;
                case 1:
                    return ExecuteCommand(shell, commands.First(), args, input, output, error);
                case > 1:
                    Console.WriteLine($"Ambiguous Command: \"{command}\"");
                    break;
            }

            return 1;
        }

        public void Help(IShell shell, string[] args)
        {
            var commands = commandCache.Values.Where(t => CheckFilter(t, shell)).Select(m => m.GetCustomAttribute<SubCommandAttribute>())
                .Where(a => a != null).Select(a => new Tuple<string, string>(a.BaseCommand, a.HelpText)).ToList();

            ConsoleFormatHelper.WriteSpacedTuples(commands, shell.Output);
        }

        public string Suggest(IShell shell, string[] commandLine)
        {
            if (commandLine.Length == 0)
            {
                return string.Empty;
            }
            var command = commandLine[0];
            var args = commandLine.Skip(1).ToArray();

            var commands = GetCommands(command, shell);

            if (commands.Count == 1)
            {
                if (args.Length == 0)
                {
                    var attribute = commands[0].GetCustomAttribute<SubCommandAttribute>();
                    if (attribute != null)
                    {
                        return attribute.BaseCommand;
                    }
                }
                else
                {
                    var instance = GetCommandInstance(commands.First(), shell);
                    return $"{command} {instance.Suggest(args)}";
                }
            }

            return string.Join(' ', commandLine);
        }

        // TODO this should be more efficient
        private bool CheckFilter(Type type, IShell shell)
        {
            if (type.IsAssignableTo(typeof(IFilteredCommand)))
            {
                var cmd = (IFilteredCommand) GetCommandInstance(type, shell);
                return cmd.ShouldShow(shell);
            }
            return true;
        }

        private List<Type> GetCommands(string command, IShell shell)
        {
            var commands = commandCache.Where(kv => kv.Key.StartsWith(command)).Select(kv => kv.Value).Where(t => CheckFilter(t, shell)).ToList();
            return commands;
        }

        private ICommand GetCommandInstance(Type commandType, IShell shell)
        {
            var constructorParams = commandType.GetConstructors().First().GetParameters()
                .Select(p => p.ParameterType == typeof(IShell) ? shell : serviceProvider.GetService(p.ParameterType)).ToArray();

            var commandInstance = (ICommand)Activator.CreateInstance(commandType, constructorParams);

            return commandInstance;
        }

        private int ExecuteCommand(IShell shell, Type commandType, string[] args, TextReader input, TextWriter output, TextWriter error)
        {
            if (!CheckFilter(commandType, shell))
            {
                return 1;
            }
            try
            {
                var instance = GetCommandInstance(commandType, shell);
                
                var result = instance.Main(args, input, output, error);

                return result;
            }
            catch (Exception e)
            {
                //logger.LogError(e, $"An Error Occurred Running \"{command}\"");
                error.WriteLine(e.Message);
                error.WriteLine(e.StackTrace);
                error.Flush();
#if DEBUG
                throw;
#else
                return 1;
#endif
            }
        }
    }
}