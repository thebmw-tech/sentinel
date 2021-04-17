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

        public int Execute(T instance, IShell shell, string[] commandLine, TextReader input, TextWriter output, TextWriter error)
        {
            var command = commandLine[0];
            var args = commandLine.Skip(1).ToArray();

            var commands = GetCommands(command);

            switch (commands.Count)
            {
                case 0:
                    Console.WriteLine($"Command Not Found: \"{command}\"");
                    break;
                case 1:
                    return ExecuteCommand(instance, commands.First(), args, input, output, error);
                case > 1:
                    Console.WriteLine($"Ambiguous Command: \"{command}\"");
                    break;
            }

            return 1;
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
            var commands = commandCache.Where(kv => kv.Key.StartsWith(command)).Select(kv => kv.Value).ToList();
            return commands;
        }

        private int ExecuteCommand(T instance, MethodInfo commandMethod, string[] args, TextReader input, TextWriter output, TextWriter error)
        {
            try
            {
                var result = commandMethod.Invoke(instance, new object[] { args, input, output, error });

                if (result != null)
                    return (int) result;

                return 0;
            }
            catch (Exception e)
            {
                //logger.LogError(e, $"An Error Occurred Running \"{command}\"");
                error.WriteLine(e.Message);
                error.WriteLine(e.StackTrace);
                error.Flush();
                return 1;
            }
        }
    }
}