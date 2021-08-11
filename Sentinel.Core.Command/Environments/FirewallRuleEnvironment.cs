using Sentinel.Core.Command.Enums;
using Sentinel.Core.Command.Interfaces;

namespace Sentinel.Core.Environments
{
    public class FirewallRuleEnvironment : IEnvironmentSetup
    {
        public void Cleanup(IShell shell)
        {
            shell.SYS_SetCommandMode(CommandMode.FirewallTable);
        }

        public string GetPrompt(IShell shell, string hostname)
        {
            throw new System.NotImplementedException();
        }

        public string[] Setup(IShell shell, string[] args)
        {
            shell.SYS_SetCommandMode(CommandMode.FirewallRule);
            return args;
        }
    }
}