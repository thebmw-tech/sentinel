using System.Collections.Generic;
using System.IO;
using Sentinel.Core.Command.Enums;
using Sentinel.Core.Command.Models;

namespace Sentinel.Core.Command.Interfaces
{
    public interface IShell
    {
        IDictionary<string, object> Environment { get; }
        CommandMode CommandMode { get; }

        void SYS_SetCommandMode(CommandMode commandMode);
        void SYS_ExitShell();

        TextWriter Output { get; }
        TextWriter Error { get; }

        T GetEnvironment<T>(string key);

    }
}