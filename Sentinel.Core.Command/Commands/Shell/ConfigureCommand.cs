using Sentinel.Core.Command.Attributes;
using Sentinel.Core.Command.Enums;
using Sentinel.Core.Command.Interfaces;
using System.IO;
using Sentinel.Core.Services.Interfaces;

namespace Sentinel.Core.Command.Commands.Shell
{
    [Command(CommandMode.Shell, "configure", "Enter Configuration Mode")]
    public class ConfigureCommand : BaseCommand
    {
        private readonly IRevisionService revisionService;

        public ConfigureCommand(IShell shell, IRevisionService revisionService) : base(shell)
        {
            this.revisionService = revisionService;
        }

        public override int Main(string[] args, TextReader input, TextWriter output, TextWriter error)
        {
            var revision = revisionService.CreateRevisionForEditing();
            shell.Environment["CONFIG_REVISION_ID"] = revision.Id;

            shell.SYS_SetCommandMode(CommandMode.Configuration);

            return 0;
        }

        public override string Suggest(string[] args)
        {
            return "configure";
        }
    }
}