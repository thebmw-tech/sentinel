using System;
using Sentinel.Core.Command.Enums;
using Sentinel.Core.Command.Interfaces;
using Sentinel.Core.Environments;

namespace Sentinel.Core.Factories
{
    public class EnvironmentSetupFactory
    {
        private IServiceProvider serviceProvider;

        public EnvironmentSetupFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IEnvironmentSetup Build(CommandMode commandMode)
        {
            switch (commandMode)
            {
                case CommandMode.Interface:
                    return (IEnvironmentSetup) serviceProvider.GetService(typeof(InterfaceEnvironment));
            }

            throw new NotImplementedException();
        }
    }
}