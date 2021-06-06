namespace Sentinel.Core
{
    public partial class SentinelConfiguration
    {
        public string DatabaseProvider { get; set; }
        public string DatabaseConnectionString { get; set; }

        public string[] EnabledGenerators { get; set; }
    }
}