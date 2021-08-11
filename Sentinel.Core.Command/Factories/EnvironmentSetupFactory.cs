using System;
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
            switch (commandMode)
            {
                case CommandMode.Shell:
                    return (IEnvironmentSetup) serviceProvider.GetService(typeof(ShellEnvironment));
                case CommandMode.Configuration:
                    return (IEnvironmentSetup) serviceProvider.GetService(typeof(ConfigurationEnvironment));
                case CommandMode.Interface:
                    return (IEnvironmentSetup) serviceProvider.GetService(typeof(InterfaceEnvironment));
                case CommandMode.FirewallTable:
                    return (IEnvironmentSetup) serviceProvider.GetService(typeof(FirewallTableEnvironment));
                case CommandMode.FirewallRule:
                    return (IEnvironmentSetup) serviceProvider.GetService(typeof(FirewallRuleEnvironment));
            }

            throw new NotImplementedException();
        }
    }
}