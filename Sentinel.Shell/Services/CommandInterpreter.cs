using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sentinel.Shell.Attributes;
using Sentinel.Shell.Enums;
using Sentinel.Shell.Helpers;
using Sentinel.Shell.Interfaces;

namespace Sentinel.Shell.Services
{
    public class CommandInterpreter
    {
        private Dictionary<CommandMode, Dictionary<string, Type>> commandCache;

        private readonly IServiceProvider serviceProvider;

        public CommandInterpreter(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;

            commandCache = new Dictionary<CommandMode, Dictionary<string, Type>>();

            var commandTypes = Assembly.GetAssembly(GetType()).GetTypes().Where(t => t.GetInterfaces().Contains(typeof(ICommand)));
            foreach (var commandType in commandTypes)
            {
                var attribute = commandType.GetCustomAttribute<CommandAttribute>();
                if (!commandCache.ContainsKey(attribute.Mode))
                {
                    commandCache.Add(attribute.Mode, new Dictionary<string, Type>());
                }

                commandCache[attribute.Mode].Add(attribute.BaseCommand, commandType);
            }
        }

        public CommandReturn Execute(CommandMode mode, string command)
        {
            var commands = GetCommands(mode, command);

            switch (commands.Count)
            {
                case 0:
                    Console.WriteLine($"Command Not Found: \"{command}\"");
                    break;
                case 1:
                    return ExecuteCommand(commands.First(), command);
                case > 1:
                    Console.WriteLine($"Ambiguous Command: \"{command}\"");
                    break;
            }

            return CommandReturn.Error;
        }

        public void Help(CommandMode mode, string command)
        {
            var commandAttributes = GetAttributesForCommands(commandCache[mode].Values.ToList())
                .Select(a => new Tuple<string, string>(a.BaseCommand, a.HelpText))
                .ToList();

            ConsoleFormatHelper.WriteSpacedTuples(commandAttributes);
        }

        public string Suggest(CommandMode mode, string command)
        {
            var commands = GetCommands(mode, command);
            switch (commands.Count)
            {
                case 1:
                    var commandInstance = GetCommandInstance(commands[0]);
                    return commandInstance.Suggest(command);
                case > 1:
                    Console.WriteLine();
                    var commandAttributes = GetAttributesForCommands(commands)
                        .Select(a => a.BaseCommand)
                        .ToList();

                    //string newCommand = commandAttributes.DefaultIfEmpty(command).FirstOrDefault();

                    //for (int i = 1; i < commandAttributes.Count; i++)
                    //{
                    //    var o = newCommand.;
                    //    var s = commandAttributes[i];
                    //    for (int c = 0; c < newCommand.Length && c < s.Length)
                    //}

                    break;
                case 0:
                    Console.Write('\a');
                    break;
            }
            return command;
        }

        private List<Type> GetCommands(CommandMode mode, string command)
        {
            var commandParts = command.Split(' ');
            if (commandParts.Length == 0)
            {
                return new List<Type>();
            }

            var commands = commandCache[mode].Where(kv => kv.Key.StartsWith(commandParts[0])).Select(kv => kv.Value).ToList();
            return commands;
        }

        private ICommand GetCommandInstance(Type commandType)
        {
            var constructorParams = commandType.GetConstructors().First().GetParameters()
                .Select(p => serviceProvider.GetService(p.ParameterType)).ToArray();

            var commandInstance = (ICommand)Activator.CreateInstance(commandType, constructorParams);

            return commandInstance;
        }

        private CommandReturn ExecuteCommand(Type commandType, string command)
        {
            var commandInstance = GetCommandInstance(commandType);

            return commandInstance.Execute(command);
        }

        private List<CommandAttribute> GetAttributesForCommands(List<Type> commandTypes)
        {
            var attributes = commandTypes.Select(t => t.GetCustomAttribute<CommandAttribute>()).Where(a => a != null).ToList();
            return attributes;
        }
    }
}