using System;
using Sentinel.Core.Entities;
using Sentinel.Core.Enums;

namespace Sentinel.Test.Builders
{
    public class InterfaceBuilder
    {
        public static Interface Build(string name, string desc = null, IpConfigurationTypeV4 type4 = IpConfigurationTypeV4.None,
            string address4 = null, byte? mask4 = null, IpConfigurationTypeV6 type6 = IpConfigurationTypeV6.None,
            string address6 = null, byte? mask6 = null, Guid? inFw = null, Guid? outFw = null, Guid? localFw = null)
        {
            return new Interface()
            {
                Name = name,
                Description = desc,
                Enabled = true,
                InterfaceType = InterfaceType.Ethernet,
                IPv4ConfigurationType = type4,
                IPv4Address = address4,
                IPv4SubnetMask = mask4,
                IPv6ConfigurationType = type6,
                IPv6Address = address6,
                IPv6SubnetMask = mask6,
                LocalFirewallTableId = localFw,
                InboundFirewallTableId = inFw,
                OutboundFirewallTableId = outFw
            };
        }
    }
}