using System;
using Sentinel.Shell.Enums;

namespace Sentinel.Shell.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandAttribute : Attribute
    {
        public CommandMode Mode { get; set; }
        public string BaseCommand { get; set; }
        public string HelpText { get; set; }

        public CommandAttribute(CommandMode mode, string baseCommand, string helpText)
        {
            this.Mode = mode;
            this.BaseCommand = baseCommand;
            this.HelpText = helpText;
        }
    }
}