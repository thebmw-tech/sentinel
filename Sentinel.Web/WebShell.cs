using Sentinel.Core.Command.Enums;
using Sentinel.Core.Command.Interfaces;
using Sentinel.Core.Command.Models;
using System.Collections.Generic;
using System.IO;

namespace Sentinel.Web
{
    public class WebShell : IShell
    {

        public WebShell(TextWriter output, TextWriter error)
        {
            Output = output;
            Error = error;

            Environment = new Dictionary<string, object>();
        }

        public CommandMode CommandMode { get; set; }

        public TextWriter Error { get; private set; }

        public Dictionary<string, object> Environment { get; set; }

        public TextWriter Output { get; private set; }

        public void SYS_ExitShell()
        {
            
        }

        public void SYS_SetCommandMode(CommandMode commandMode)
        {
            CommandMode = commandMode;
        }
    }
}