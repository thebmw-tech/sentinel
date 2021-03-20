using System.Collections.Generic;

namespace Sentinel.Core.Models.Configuration.Netplan
{
    public class Ethernet
    {
        public Dictionary<string, string> Match { get; set; }
        public bool Dhcp4 { get; set; }
        public List<string> Addresses { get; set; }
        public string Gateway4 { get; set; }
        public string Gateway6 { get; set; }
        public string Renderer { get; set; }
    }
}