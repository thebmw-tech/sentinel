using System.IO;
using Sentinel.Core.Command.Attributes;
using Sentinel.Core.Command.Enums;
using Sentinel.Core.Command.Interfaces;

namespace Sentinel.Core.Command.Commands.Shell
{
    [Command(CommandMode.Shell, "template", "Help")]
    public class TemplateCommand : BaseCommand
    {
        public TemplateCommand(IShell shell) : base(shell)
        {

        }

        public override int Main(string[] args, TextReader input, TextWriter output, TextWriter error)
        {
            output.WriteLine("Hello World");
            output.WriteLine("Goodbye World");
            output.WriteLine("Test String");
            output.WriteLine("eth0");

            return 0;
        }

        public override string Suggest(string[] args)
        {
            throw new System.NotImplementedException();
        }
    }
}