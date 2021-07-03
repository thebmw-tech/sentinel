using System;
using System.Linq;
using Sentinel.Core.Command.Interfaces;

namespace Sentinel.Core.Helpers
{
    public static class CommandHelper
    {
        public static ICommand GetCommandInstance(Type commandType, IShell shell, IServiceProvider serviceProvider)
        {
            var constructorParams = commandType.GetConstructors().First().GetParameters()
                .Select(p => p.ParameterType == typeof(IShell) ? shell : serviceProvider.GetService(p.ParameterType)).ToArray();

            var commandInstance = (ICommand)Activator.CreateInstance(commandType, constructorParams);

            return commandInstance;
        }
    }
}