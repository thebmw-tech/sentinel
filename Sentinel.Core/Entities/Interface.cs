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


        public Guid? InboundFirewallTableId { get; set; }
        public Guid? OutboundFirewallTableId { get; set; }
        public Guid? LocalFirewallTableId { get; set; }
    }
}