using Sentinel.Core;

namespace Sentinel.CLI.Configuration
{
    public class ConfigFile : IModule
    {
        public void Main(string[] args)
        {
            // TODO make this have more
            SentinelConfiguration.SaveDefaultConfigToFile(args[0]);
        }
    }
}