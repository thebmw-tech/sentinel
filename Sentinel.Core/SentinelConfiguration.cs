namespace Sentinel.Core
{
    public partial class SentinelConfiguration
    {
        public string DatabaseProvider { get; set; }
        public string DatabaseConnectionString { get; set; }

        public string HangfireDatabaseProvider { get; set; }
        public string HangfireDatabaseConnectionString { get; set; }

        public string[] EnabledGenerators { get; set; }
    }
}