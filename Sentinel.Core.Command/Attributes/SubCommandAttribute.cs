using System;

namespace Sentinel.Core.Command.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class SubCommandAttribute : Attribute
    {
        public string BaseCommand { get; set; }
        public string HelpText { get; set; }

        public SubCommandAttribute(string baseCommand, string helpText)
        {
            this.BaseCommand = baseCommand;
            this.HelpText = helpText;
        }
    }
}