using System;
using Sentinel.Core.Enums;

namespace Sentinel.Core.Entities
{
    public class Route : BaseVersionedEntity<Route>
    {
        public string Address { get; set; }
        public byte SubnetMask { get; set; }
        public IPVersion Version { get; set; }
        
        public Guid GatewayId { get; set; }

        public string Description { get; set; }
    }
}