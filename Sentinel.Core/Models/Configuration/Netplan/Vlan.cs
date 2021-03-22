using System;
using System.Collections.Generic;

namespace Sentinel.Core.Models.Configuration.Netplan
{
    public class Vlan
    {
        public int Id { get; set; }
        public String Link { get; set; }
        public bool Dhcp4 { get; set; }
        public List<string> Addresses { get; set; }
        public string Gateway4 { get; set; }
        public string Gateway6 { get; set; }
    }
}