using System;
using Sentinel.Core.Command.Enums;
using Sentinel.Core.Command.Models;

namespace Sentinel.Web.Models
{
    public class WebShellExecutionRequest
    {
        public string Command { get; set; }

        public CommandMode CommandMode { get; set; }
        public ShellContext Context { get; set; }
    }
}