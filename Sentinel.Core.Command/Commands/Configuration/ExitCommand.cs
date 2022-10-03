using Sentinel.Core.Command.Attributes;
using Sentinel.Core.Command.Enums;
using Sentinel.Core.Command.Interfaces;
using Sentinel.Core.Services.Interfaces;
using System.IO;
using System.Linq;

namespace Sentinel.Core.Command.Commands.Configuration
{
    [Command(CommandMode.Configuration, "exit", "Exit Configuration Mode")]
    public class ExitCommand : BaseCommand
    {
        private IRevisionService revisionService;
        public ExitCommand(IShell shell, IRevisionService revisionService) : base(shell)
        {
            this.revisionService = revisionService;
        }

        public override int Main(string[] args, TextReader input, TextWriter output, TextWriter error)
        {
            if (args.Length == 0)
            {
                var revisionId = (int)shell.Environment["CONFIG_REVISION_ID"];
                var revision = revisionService.GetRevisionById(revisionId);
                if (revision.HasChanges && !revision.CommitDate.HasValue)
                {
                    error.WriteLine("Please \"commit\" or \"exit discard\" to leave configuration mode.");
                    return 1;
                }

                if (revision.HasChanges && !revision.ConfirmDate.HasValue)
                {
                    error.WriteLine("You have not confirmed configuration. Please \"confirm\" or \"rollback\" before exiting.");
                    return 1;
                }

                revisionService.UnlockRevision(revisionId);

                var keysToDelete = shell.Environment.Keys.Where(s => s.StartsWith("CONFIG"));
                foreach (var key in keysToDelete)
                {
                    shell.Environment.Remove(key);
                }

                shell.SYS_SetCommandMode(CommandMode.Shell);
            }

            return 0;
        }

        public override string Suggest(string[] args)
        {
            throw new System.NotImplementedException();
        }
    }
}