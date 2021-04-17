using System;
using Sentinel.Core.Enums;

namespace Sentinel.Core.Entities
{
    public class Interface : BaseVersionedEntity<Interface>
    {
        public string Name { get; set; }

        public InterfaceType InterfaceType { get; set; }

        public string Description { get; set; }

        public long? SpoofedMAC { get; set; }


        public IpConfigurationTypeV4 IPv4ConfigurationType { get; set; }
        public string IPv4Address { get; set; }
        public byte? IPv4SubnetMask { get; set; }

        public IpConfigurationTypeV6 IPv6ConfigurationType { get; set; }
        public string IPv6Address { get; set; }
        public byte? IPv6SubnetMask { get; set; }

        public Guid? InboundFirewallTableId { get; set; }
        public Guid? OutboundFirewallTableId { get; set; }
        public Guid? LocalFirewallTableId { get; set; }
    }
}