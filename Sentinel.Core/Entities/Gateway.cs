using System;
using System.Collections.Generic;
using Sentinel.Core.Enums;

namespace Sentinel.Core.Entities
{
    public class Gateway : BaseVersionedEntity<Gateway>
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public GatewayType GatewayType { get; set; }

        public string InterfaceName { get; set; }
        public virtual Interface Interface { get; set; }

        public string IPAddress { get; set; }
        public IPVersion Version { get; set; }

        
    }
}