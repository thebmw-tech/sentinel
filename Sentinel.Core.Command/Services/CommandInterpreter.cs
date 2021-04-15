using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Logging;
using Sentinel.Core.Command.Attributes;
using Sentinel.Core.Command.Enums;
using Sentinel.Core.Command.Interfaces;
using Sentinel.Core.Command.Models;
using Sentinel.Core.Helpers;

namespace Sentinel.Core.Command.Services
{
    public class CommandInterpreter
    {
        private Dictionary<CommandMode, Dictionary<string, Type>> commandCache;

        private readonly IServiceProvider serviceProvider;
        private readonly ILogger<CommandInterpreter> logger;

        public CommandInterpreter(IServiceProvider serviceProvider, ILogger<CommandInterpreter> logger)
        {
            this.serviceProvider = serviceProvider;
            this.logger = logger;

            commandCache = new Dictionary<CommandMode, Dictionary<string, Type>>();

            var commandTypes = Assembly.GetAssembly(GetType()).GetTypes().Where(t => t.GetInterfaces().Contains(typeof(ICommand)) && !t.IsAbstract);
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

        public int Execute(IShell shell, CommandMode mode, string commandLine)
        {
            var commandsWithArgs = ParseCommandLine(commandLine);

            var stringBuilder = new StringBuilder();
            int exitCode = -1;

            for (int i = 0; i < commandsWithArgs.Count; i++)
            {
                var commandWithArgs = commandsWithArgs[i];

                var input = TextReader.Null;
                var output = TextWriter.Null;
                var error = shell.Error; // TODO support redirecting error output

                // Setup inputs and outputs
                if (commandsWithArgs.Count > 1)
                {
                    var str = stringBuilder.ToString();
                    input = new StringReader(str);
                    stringBuilder = new StringBuilder();
                    output = new StringWriter(stringBuilder);
                }

                if (i == commandsWithArgs.Count - 1)
                {
                    output = shell.Output;
                }

                // Find the command and run it;
                var commands = GetCommands(mode, commandWithArgs.Item1);

                switch (commands.Count)
                {
                    case 0:
                        shell.Error.WriteLine($"Command Not Found: \"{commandWithArgs.Item1}\"");
                        return -1;
                    case 1:
                        exitCode = ExecuteCommand(commands.First(), shell, commandWithArgs.Item2, input, output, error);
                        break;
                    case > 1:
                        shell.Error.WriteLine($"Ambiguous Command: \"{commandWithArgs.Item1}\"");
                        return -1;
                }
            }

            return exitCode;
        }

        public void Help(IShell shell, CommandMode mode, string command)
        {
            if (String.IsNullOrWhiteSpace(command))
            {
                var commandAttributes = GetAttributesForCommands(commandCache[mode].Values.ToList())
                    .Select(a => new Tuple<string, string>(a.BaseCommand, a.HelpText))
                    .ToList();

                ConsoleFormatHelper.WriteSpacedTuples(commandAttributes);
            }
            else
            {
                var commands = GetCommands(mode, command);

                switch (commands.Count)
                {
                    case 1:
                        //var commandInstance = GetCommandInstance(commands[0], shell);
                        //commandInstance.Help(command);
                        break;
                    case > 1:
                        Console.WriteLine();

                        var commandStrings = commands.Select(c => c.GetCustomAttribute<CommandAttribute>())
                            .Where(s => s != null).Select(s => s.BaseCommand).ToList();

                        break;
                        //return HelperFunctions.LCDString(commandStrings);
                    case 0:
                        Console.Write('\a');
                        break;
                }
            }
        }

        public string Suggest(IShell shell, CommandMode mode, string command)
        {
            if (command.Trim().Length == 0)
            {
                return command;
            }

            var commandsWithArgs = ParseCommandLine(command);
            var commandWithArgs = commandsWithArgs.Last();

            var commands = GetCommands(mode, commandWithArgs.Item1);
            switch (commands.Count)
            {
                case 1:
                    var commandInstance = GetCommandInstance(commands.First(), shell);
                    return commandInstance.Suggest(commandWithArgs.Item2);
                case > 1:
                    Console.WriteLine();

                    var commandStrings = commands.Select(c => c.GetCustomAttribute<CommandAttribute>())
                        .Where(s => s != null).Select(s => s.BaseCommand).ToList();
                    
                    return HelperFunctions.LCDString(commandStrings);
                case 0:
                    shell.Output.Write('\a');
                    break;
            }
            return command;
        }


        private List<Tuple<string, string[]>> ParseCommandLine(string commandLine)
        {
            var commandsWithArgs = commandLine.Split('|');
            var parsedCommands = commandsWithArgs.Select(ParseCommandWithArgs).ToList();
            return parsedCommands;
        }

        private Tuple<string, string[]> ParseCommandWithArgs(string commandWithArgs)
        {
            commandWithArgs = commandWithArgs.Trim();
            var firstSpaceIndex = commandWithArgs.IndexOf(' ');
            if (firstSpaceIndex == -1)
            {
                return new Tuple<string, string[]>(commandWithArgs, new string[] {});
            }
            var command = commandWithArgs.Substring(0, firstSpaceIndex);
            var argsString = commandWithArgs.Substring(firstSpaceIndex + 1);
            var args = argsString.ParseArguments().ToArray();
            return new Tuple<string, string[]>(command, args);
        }






        private List<Type> GetCommands(CommandMode mode, string command)
        {
            var commands = commandCache.Where(kv => kv.Key == mode || kv.Key == CommandMode.Any)
                .Select(kv => kv.Value).SelectMany(s => s).Where(kv => kv.Key.StartsWith(command))
                .Select(kv => kv.Value).ToList();

            return commands;
        }

        private ICommand GetCommandInstance(Type commandType, IShell shell)
        {
            var constructorParams = commandType.GetConstructors().First().GetParameters()
                .Select(p =>  p.ParameterType == typeof(IShell) ? shell : serviceProvider.GetService(p.ParameterType)).ToArray();

            var commandInstance = (ICommand)Activator.CreateInstance(commandType, constructorParams);

            return commandInstance;
        }

        private int ExecuteCommand(Type commandType, IShell shell, string[] args, TextReader input, TextWriter output, TextWriter error)
        {
            try
            {
                var commandInstance = GetCommandInstance(commandType, shell);

                return commandInstance.Main(args, input, output, error);
            }
            catch (Exception e)
            {
                shell.Error.WriteLine(e.Message);
                shell.Error.WriteLine(e.StackTrace);
                shell.Error.Flush();
                return -2;
            }
        }

        private List<CommandAttribute> GetAttributesForCommands(List<Type> commandTypes)
        {
            var attributes = commandTypes.Select(t => t.GetCustomAttribute<CommandAttribute>()).Where(a => a != null).ToList();
            return attributes;
        }
    }
}