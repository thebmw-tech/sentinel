using System;

namespace Sentinel.Core.Entities
{
    public class SystemConfiguration : BaseVersionedEntity<SystemConfiguration>
    {
        public String Hostname { get; set; }
        public String Domain { get; set; }

        // Default Gateways
        public Guid? IPv4DefaultGateway { get; set; }
        public Guid? IPv6DefaultGateway { get; set; }

        
    }
}