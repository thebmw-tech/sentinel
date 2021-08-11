using Sentinel.Core.Command.Interfaces;

namespace Sentinel.Core.Environments
{
    public class ShellEnvironment : IEnvironmentSetup
    {
        public void Cleanup(IShell shell)
        {
            shell.SYS_ExitShell();
        }

        public string[] Setup(IShell shell, string[] args)
        {
            throw new System.NotImplementedException();
        }
    }
}