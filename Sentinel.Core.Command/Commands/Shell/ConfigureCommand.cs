using Sentinel.Core.Command.Attributes;
using Sentinel.Core.Command.Enums;
using Sentinel.Core.Command.Interfaces;
using Sentinel.Core.Commands;
using System.IO;

namespace Sentinel.Core.Command.Commands.Shell
{
    [Command(CommandMode.Shell, "configure", "Enter Configuration Mode")]
    public class ConfigureCommand : BaseCommand
    {
        public ConfigureCommand(IShell shell) : base(shell)
        {

        }

        public override int Main(string[] args, TextReader input, TextWriter output, TextWriter error)
        {
            shell.SYS_SetCommandMode(CommandMode.Configuration);

            // TODO we should be creating a revision and setting it up here.

            return 0;
        }

        public override string Suggest(string[] args)
        {
            return "configure";
        }
    }
}