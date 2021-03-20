using System.Collections.Generic;

namespace Sentinel.Core.Models.Configuration.Netplan
{
    public class Network
    {
        public int Version { get; set; }
        public string Renderer { get; set; }
        
        public Dictionary<string, Ethernet> Ethernets { get; set; }
    }
}