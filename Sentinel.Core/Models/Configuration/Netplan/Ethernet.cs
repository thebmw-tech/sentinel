using System.Collections.Generic;

namespace Sentinel.Core.Models.Configuration.Netplan
{
    public class Ethernet : Interface
    {
        public Dictionary<string, string> Match { get; set; }
        
        
    }
}