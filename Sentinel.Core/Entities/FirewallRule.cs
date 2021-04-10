using System;
using Sentinel.Core.Enums;

namespace Sentinel.Core.Entities
{
    public class FirewallRule : BaseVersionedEntity<FirewallRule>
    {
        public Guid Id { get; set; }
        public int Order { get; set; }

        public FirewallAction Action { get; set; }
        public String InterfaceName { get; set; }
        public IPVersion IpVersion { get; set; }
        public IPProtocol Protocol { get; set; }

        public bool InvertSourceMatch { get; set; }
        public string SourceAddress { get; set; }
        public byte? SourceSubnetMask { get; set; }

        public short? SourcePortRangeStart { get; set; }
        public short? SourcePortRangeEnd { get; set; }

        public bool InvertDestinationMatch { get; set; }
        public string DestinationAddress { get; set; }
        public byte? DestinationSubnetMask { get; set; }

        public short? DestinationPortRangeStart { get; set; }
        public short? DestinationPortRangeEnd { get; set; }


        public bool Log { get; set; }
        public string Description { get; set; }

    }
}