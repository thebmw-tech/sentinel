using System;

namespace Sentinel.Shell
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandAttribute : Attribute
    {
        public CommandMode Mode { get; set; }
        public string BaseCommand { get; set; }

        public CommandAttribute(CommandMode mode, string baseCommand)
        {
            this.Mode = mode;
            this.BaseCommand = baseCommand;
        }
    }
}