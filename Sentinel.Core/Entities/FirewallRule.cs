using System;
using Sentinel.Core.Enums;

namespace Sentinel.Core.Entities
{
    public class FirewallRule : BaseRule<FirewallRule>
    {
        public Guid FirewallTableId { get; set; }
        public FirewallAction Action { get; set; }
    }
}