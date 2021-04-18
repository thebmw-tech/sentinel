using System;
using Sentinel.Core.Entities;
using Sentinel.Core.Enums;

namespace Sentinel.Test.Builders
{
    public class InterfaceBuilder
    {
        public static Interface Build(string name, string desc = null, string address4 = null, byte? mask4 = null, string address6 = null, byte? mask6 = null, Guid? inFw = null, Guid? outFw = null, Guid? localFw = null)
        {
            return new()
            {
                Name = name,
                Description = desc,
                Enabled = true,
                InterfaceType = InterfaceType.Ethernet,
                LocalFirewallTableId = localFw,
                InboundFirewallTableId = inFw,
                OutboundFirewallTableId = outFw
            };
        }
    }
}