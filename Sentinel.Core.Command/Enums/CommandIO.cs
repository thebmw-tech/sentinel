using System;

namespace Sentinel.Core.Command.Enums
{
    [Flags]
    public enum CommandIO
    {
        Out = 1,
        Error = 2,
        Default = Out | Error,
        In = 4,
        All = Default | In,
    }
}