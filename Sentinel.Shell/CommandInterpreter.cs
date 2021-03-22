using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sentinel.Shell
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

        public void Execute(CommandMode mode, string command)
        {
            var commands = GetCommands(mode, command);

            switch (commands.Count)
            {
                case 0:
                    
                    break;
                case 1:
                    ExecuteCommand(commands.First(), command);
                    break;
                case > 1:

                    break;
            }

        }

        public void Help(CommandMode mode, string command)
        {
            Console.WriteLine($"Help for \"{command}\"");
        }

        public string Suggest(CommandMode mode, string command)
        {
            return "";
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

        private void ExecuteCommand(Type commandType, string command)
        {
            var commandInstance = GetCommandInstance(commandType);

            commandInstance.Execute(command);
        }
    }
}