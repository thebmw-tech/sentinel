using Sentinel.Core.Command.Interfaces;

namespace Sentinel.Core.Environments
{
    public class ShellEnvironment : IEnvironmentSetup
    {
        public void Cleanup(IShell shell, string[] args)
        {
            shell.SYS_ExitShell();
        }

        public string GetPrompt(IShell shell, string hostname)
        {
            return $"{hostname}>";
        }

        public string[] Setup(IShell shell, string[] args)
        {
            throw new System.NotImplementedException();
        }
    }
}