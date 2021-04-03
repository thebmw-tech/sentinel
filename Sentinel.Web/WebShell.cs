using Sentinel.Core.Command.Enums;
using Sentinel.Core.Command.Interfaces;
using Sentinel.Core.Command.Models;
using System.IO;

namespace Sentinel.Web
{
    public class WebShell : IShell
    {

        public WebShell(TextWriter output, TextWriter error)
        {
            Out = output;
            Error = error;
        }

        public ShellContext Context { get; set; }
        public CommandMode CommandMode { get; set; }

        public TextWriter Out { get; private set; }

        public TextWriter Error { get; private set; }
    }
}