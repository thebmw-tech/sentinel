using Sentinel.Core.Command.Interfaces;

namespace Sentinel.Core.Environments
{
    public class ConfigurationEnvironment : IEnvironmentSetup
    {
        public void Cleanup(IShell shell, string[] args)
        {
            throw new System.NotImplementedException();
        }

        public string GetPrompt(IShell shell, string hostname)
        {
            var revision = shell.GetEnvironment<int>(SentinelCommandEnvironment.REVISON_ID);
            return $"{hostname}(config[r{revision:X}])#";
        }

        public string[] Setup(IShell shell, string[] args)
        {
            throw new System.NotImplementedException();
        }
    }
}