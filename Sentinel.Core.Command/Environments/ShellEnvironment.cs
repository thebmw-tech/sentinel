using Sentinel.Core.Command.Interfaces;

namespace Sentinel.Core.Environments
{
    public class ShellEnvironment : IEnvironmentSetup
    {
        public void Cleanup(IShell shell)
        {
            throw new System.NotImplementedException();
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