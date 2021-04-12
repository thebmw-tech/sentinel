using System;

namespace Sentinel.Core.Enums
{
    [Flags]
    public enum FirewallState
    {
        None = 0,
        Invalid = 1,
        New = 2,
        Established = 4,
        Related = 8
    }
}