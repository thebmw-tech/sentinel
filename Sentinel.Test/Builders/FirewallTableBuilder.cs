using System;
using Sentinel.Core.Entities;
using Sentinel.Core.Enums;

namespace Sentinel.Test.Builders
{
    public class FirewallTableBuilder
    {
        public static FirewallTable Buid(string name, FirewallAction defaultAction = FirewallAction.Block, string description = null)
        {
            return new FirewallTable()
            {
                Id = Guid.NewGuid(),
                DefaultAction = defaultAction,
                Name = name,
                Description = description,
                DefaultLog = false
            };
        }
    }
}