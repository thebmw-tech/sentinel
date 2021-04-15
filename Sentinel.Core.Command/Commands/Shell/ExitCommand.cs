using Sentinel.Core.Command.Attributes;
using Sentinel.Core.Command.Enums;
using Sentinel.Core.Command.Interfaces;
using System.IO;
using System.Threading.Tasks;

namespace Sentinel.Core.Command.Commands.Shell
{
    [Command(CommandMode.Shell, "exit", "Exits the shell.")]
    public class ExitCommand : BaseCommand
    {
        public ExitCommand(IShell shell) : base(shell)
        {

        }

        public override int Main(string[] args, TextReader input, TextWriter output, TextWriter error)
        {
            shell.SYS_ExitShell();
            return 0;
        }

        public override string Suggest(string[] args)
        {
            return "exit";
        }
    }
}