using System;

namespace Sentinel.Core.Entities
{
    public class SystemConfiguration : BaseVersionedEntity<SystemConfiguration>
    {
        public string Hostname { get; set; }
        public string Domain { get; set; }

        public string DHCPHostname { get; set; }

        public uint ShellHistoryLength { get; set; }
    }
}