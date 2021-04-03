using System.IO;
using Sentinel.Core.Command.Enums;
using Sentinel.Core.Command.Models;

namespace Sentinel.Core.Command.Interfaces
{
    public interface IShell
    {
        ShellContext Context { get; set; }
        CommandMode CommandMode { get; set; }

        TextWriter Out { get; }
        TextWriter Error { get; }
    }
}