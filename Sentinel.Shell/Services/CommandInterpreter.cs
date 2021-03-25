using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Sentinel.Shell.Attributes;
using Sentinel.Shell.Enums;
using Sentinel.Shell.Helpers;
using Sentinel.Shell.Interfaces;
using Sentinel.Shell.Models;

namespace Sentinel.Shell.Services
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

        public CommandReturn Execute(CommandMode mode, ShellContext context, string command)
        {
            var commands = GetCommands(mode, command);

            switch (commands.Count)
            {
                case 0:
                    Console.WriteLine($"Command Not Found: \"{command}\"");
                    break;
                case 1:
                    return ExecuteCommand(commands.First(), context, command);
                case > 1:
                    Console.WriteLine($"Ambiguous Command: \"{command}\"");
                    break;
            }

            return CommandReturn.Error;
        }

        public void Help(CommandMode mode, string command)
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
                        var commandInstance = GetCommandInstance(commands[0]);
                        commandInstance.Help(command);
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

                    var commandStrings = commands.Select(c => c.GetCustomAttribute<CommandAttribute>())
                        .Where(s => s != null).Select(s => s.BaseCommand).ToList();
                    
                    return HelperFunctions.LCDString(commandStrings);
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

        private CommandReturn ExecuteCommand(Type commandType, ShellContext context, string command)
        {
            try
            {
                var commandStr = HelperFunctions.GetSubCommand(command);
                var commandInstance = GetCommandInstance(commandType);

                return commandInstance.Execute(context, commandStr);
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

        private List<CommandAttribute> GetAttributesForCommands(List<Type> commandTypes)
        {
            var attributes = commandTypes.Select(t => t.GetCustomAttribute<CommandAttribute>()).Where(a => a != null).ToList();
            return attributes;
        }
    }
}