using System;
using System.Collections.Generic;

namespace Sentinel.Core.Models.Configuration.Netplan
{
    public class Vlan : Interface
    {
        public int Id { get; set; }
        public String Link { get; set; }
        
    }
}