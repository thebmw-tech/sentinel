using System.Collections.Generic;

namespace Sentinel.Core.Models.Configuration.Netplan
{
    public class Interface
    {
        public bool Dhcp4 { get; set; }
        public bool Dhcp6 { get; set; }
        public List<string> Addresses { get; set; }
        public string Gateway4 { get; set; }
        public string Gateway6 { get; set; }

        public string Renderer { get; set; }

        public List<Route> Routes { get; set; }
    }
}