using Sentinel.Core.Command.Attributes;
using Sentinel.Core.Command.Enums;
using Sentinel.Core.Command.Interfaces;
using System.IO;

namespace Sentinel.Core.Command.Commands.Interface
{
    [Command(CommandMode.Interface, "exit", "Exit from interface configuration")]
    public class ExitCommand : BaseCommand
    {
        public ExitCommand(IShell shell) : base(shell)
        {

        }

        public override int Main(string[] args, TextReader input, TextWriter output, TextWriter error)
        {
            // TODO this should check and save the interface
            shell.SYS_SetCommandMode(CommandMode.Configuration);
            return 0;
        }

        public override string Suggest(string[] args)
        {
            throw new System.NotImplementedException();
        }
    }
}