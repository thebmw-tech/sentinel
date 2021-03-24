using System;

namespace Sentinel.Core.Entities
{
    public class SystemConfiguration : BaseVersionedEntity<SystemConfiguration>
    {
        public String Hostname { get; set; }
    }
}