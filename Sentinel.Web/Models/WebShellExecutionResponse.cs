using System;
using Sentinel.Core.Command.Enums;
using Sentinel.Core.Command.Models;

namespace Sentinel.Web.Models
{
    public class WebShellExecutionResponse
    {
        public string Output { get; set; }
        public string Error { get; set; }

        public CommandReturn Return { get; set; }

        public CommandMode CommandMode { get; set; }
        public ShellContext Context { get; set; }
    }
}