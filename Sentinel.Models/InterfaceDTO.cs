using System;

namespace Sentinel.Models
{
    public class InterfaceDTO
    {
        public bool Enabled { get; set; }
        public string Name { get; set; }

        public string InterfaceType { get; set; }

        public string Description { get; set; }

        public long? SpoofedMAC { get; set; }


        public string IPv4ConfigurationType { get; set; }
        public string IPv4Address { get; set; }
        public byte? IPv4SubnetMask { get; set; }
        public Guid? IPv4GatewayId { get; set; }

        public string IPv6ConfigurationType { get; set; }
        public string IPv6Address { get; set; }
        public byte? IPv6SubnetMask { get; set; }
        public Guid? IPv6GatewayId { get; set; }

        public Guid? InboundFirewallTableId { get; set; }
        public Guid? OutboundFirewallTableId { get; set; }
        public Guid? LocalFirewallTableId { get; set; }
    }
}