using System;
using Sentinel.Core.Command.Enums;

namespace Sentinel.Core.Command.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandAttribute : Attribute
    {
        public CommandMode Mode { get; set; }
        public string BaseCommand { get; set; }
        public string HelpText { get; set; }
        public bool PublicCommand { get; set; }

        public CommandAttribute(CommandMode mode, string baseCommand, string helpText, bool publicCommand = true)
        {
            Mode = mode;
            BaseCommand = baseCommand;
            HelpText = helpText;
            PublicCommand = publicCommand;
        }
    }
}