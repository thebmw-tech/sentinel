using System;
using System.Collections.Generic;
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
        private Dictionary<string, MethodInfo> commandCache;

        public SubCommandInterpreter()
        {
            commandCache = new Dictionary<string, MethodInfo>();

            var methods = typeof(T).GetMethods();
            foreach (var method in methods)
            {
                var attribute = method.GetCustomAttribute(typeof(SubCommandAttribute));
                if (attribute != null)
                {
                    var subCommandAttribute = (SubCommandAttribute) attribute;
                    commandCache.Add(subCommandAttribute.BaseCommand, method);
                }
            }
        }

        public CommandReturn Execute(T instance, IShell shell, string command)
        {
            var commands = GetCommands(command);

            switch (commands.Count)
            {
                case 0:
                    Console.WriteLine($"Command Not Found: \"{command}\"");
                    break;
                case 1:
                    return ExecuteCommand(instance, commands.First(), command);
                case > 1:
                    Console.WriteLine($"Ambiguous Command: \"{command}\"");
                    break;
            }

            return CommandReturn.Error;
        }

        public void Help(IShell shell, string command)
        {
            var commands = commandCache.Values.Select(m => m.GetCustomAttribute<SubCommandAttribute>())
                .Where(a => a != null).Select(a => new Tuple<string, string>(a.BaseCommand, a.HelpText)).ToList();

            ConsoleFormatHelper.WriteSpacedTuples(commands);
        }

        public string Suggest(IShell shell, string command)
        {
            return command;
        }

        private List<MethodInfo> GetCommands(string command)
        {
            var commandParts = command.Split(' ');
            if (commandParts.Length == 0)
            {
                return new List<MethodInfo>();
            }

            var commands = commandCache.Where(kv => kv.Key.StartsWith(commandParts[0])).Select(kv => kv.Value).ToList();
            return commands;
        }

        private CommandReturn ExecuteCommand(T instance, MethodInfo commandMethod, string command)
        {
            try
            {
                var commandStr = HelperFunctions.GetSubCommand(command);
                var result = commandMethod.Invoke(instance, new object?[] { commandStr });

                if (result != null)
                    return (CommandReturn) result;

                return CommandReturn.Normal;
            }
            catch (Exception e)
            {
                //logger.LogError(e, $"An Error Occurred Running \"{command}\"");
                Console.Error.WriteLine(e.Message);
                Console.Error.WriteLine(e.StackTrace);
                Console.Error.Flush();
                return CommandReturn.Error;
            }
        }
    }
}