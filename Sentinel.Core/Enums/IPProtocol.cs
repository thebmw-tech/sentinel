using System;

namespace Sentinel.Core.Enums
{
    [Flags]
    public enum IPProtocol
    {
        Any = 0,
        TCP = 1,
        UDP = 2,
        ICMP = 4
    }
}