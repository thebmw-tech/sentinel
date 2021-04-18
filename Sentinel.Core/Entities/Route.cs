using System;
using Sentinel.Core.Enums;

namespace Sentinel.Core.Entities
{
    public class Route : BaseVersionedEntity<Route>
    {
        public RouteType RouteType { get; set; }
        public string InterfaceName { get; set; }

        public string Address { get; set; }
        public byte SubnetMask { get; set; }
        public IPVersion Version { get; set; }

        public string NextHopAddress { get; set; }

        public string Description { get; set; }
    }
}