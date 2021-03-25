using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters;
using Sentinel.Shell.Attributes;
using Sentinel.Shell.Enums;
using Sentinel.Shell.Helpers;
using Sentinel.Shell.Interfaces;
using Sentinel.Shell.Models;

namespace Sentinel.Shell.Services
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

        public CommandReturn Execute(T instance, ShellContext context, string command)
        {
            var commands = GetCommands(command);

            switch (commands.Count)
            {
                case 0:
                    Console.WriteLine($"Command Not Found: \"{command}\"");
                    break;
                case 1:
                    return ExecuteCommand(instance, commands.First(), context, command);
                case > 1:
                    Console.WriteLine($"Ambiguous Command: \"{command}\"");
                    break;
            }

            return CommandReturn.Error;
        }

        public void Help(string command)
        {
            var commands = commandCache.Values.Select(m => m.GetCustomAttribute<SubCommandAttribute>())
                .Where(a => a != null).Select(a => new Tuple<string, string>(a.BaseCommand, a.HelpText)).ToList();

            ConsoleFormatHelper.WriteSpacedTuples(commands);
        }

        public string Suggest(string command)
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

        private CommandReturn ExecuteCommand(T instance, MethodInfo commandMethod, ShellContext context, string command)
        {
            try
            {
                var commandStr = HelperFunctions.GetSubCommand(command);
                var result = commandMethod.Invoke(instance, new object?[] { context, commandStr });

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