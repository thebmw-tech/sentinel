using System;
using System.Linq;
using System.Reflection;
using Sentinel.Core.Command.Enums;
using Sentinel.Core.Command.Interfaces;
using Sentinel.Core.Environments;

namespace Sentinel.Core.Factories
{
    public class EnvironmentSetupFactory
    {
        private readonly IServiceProvider serviceProvider;

        public EnvironmentSetupFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IEnvironmentSetup Build(CommandMode commandMode)
        {
            var typeName = $"{commandMode}Environment";
            var envType = Assembly.GetAssembly(GetType())?.GetTypes().FirstOrDefault(t => t.Name == typeName);
            if (envType != null)
            {
                return (IEnvironmentSetup)serviceProvider.GetService(envType);
            }

            throw new NotImplementedException($"{typeName} is not implemented.");
        }
    }
}