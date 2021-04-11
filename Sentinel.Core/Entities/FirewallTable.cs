using System;
using Sentinel.Core.Enums;

namespace Sentinel.Core.Entities
{
    public class FirewallTable : BaseVersionedEntity<FirewallTable>
    {
        public Guid Id { get; set; }
        public FirewallAction DefaultAction { get; set; }

        public string Description { get; set; }
        public bool DefaultLog { get; set; }
    }
}