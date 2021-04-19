using System;
using Sentinel.Core.Command.Attributes;
using Sentinel.Core.Command.Enums;
using Sentinel.Core.Command.Interfaces;
using System.IO;
using Sentinel.Core.Services.Interfaces;
using Sentinel.Models;

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
            if (args.Length > 1)
            {
                error.WriteLine("Invalid number of arguments");
                return 1;
            }

            RevisionDTO revision = null;

            if (args.Length == 0)
            {
                revision = revisionService.CreateRevisionForEditing();
            }
            else
            {
                var arg = args[0];
                if ("resume".StartsWith(arg))
                {
                    revision = revisionService.ResumeEditingRevision();
                    if (revision == null)
                    {
                        error.WriteLine("Failed to resume editing");
                        return 1;
                    }
                }
            }

            if (revision == null)
            {
                error.WriteLine("Failed to get revision for editing");
                return 1;
            }

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