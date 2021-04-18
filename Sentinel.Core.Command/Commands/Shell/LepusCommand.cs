using Sentinel.Core.Command.Attributes;
using Sentinel.Core.Command.Enums;
using Sentinel.Core.Command.Interfaces;
using Sentinel.Core.Command.Services;
using System.IO;
using Sentinel.Core.Services.Interfaces;

namespace Sentinel.Core.Command.Commands.Shell
{
    [Command(CommandMode.Shell, "lepus", "", false)]
    public class LepusCommand : BaseCommand
    {
        private readonly SubCommandInterpreter<LepusCommand> subCommandInterpreter;

        public LepusCommand(IShell shell, SubCommandInterpreter<LepusCommand> subCommandInterpreter) : base(shell)
        {
            this.subCommandInterpreter = subCommandInterpreter;
        }

        public override int Main(string[] args, TextReader input, TextWriter output, TextWriter error)
        {
            output.WriteLine("THESE COMMANDS CAN BREAK THINGS.");
            output.WriteLine("Please be careful");
            return subCommandInterpreter.Execute(shell, args, input, output, error);
        }

        public override string Suggest(string[] args)
        {
            return subCommandInterpreter.Suggest(shell, args);
        }

        [SubCommand("clear_lock", "")]
        public class ClearLockCommand : BaseCommand
        {
            private readonly IRevisionService revisionService;

            public ClearLockCommand(IShell shell, IRevisionService revisionService) : base(shell)
            {
                this.revisionService = revisionService;
            }

            public override int Main(string[] args, TextReader input, TextWriter output, TextWriter error)
            {
                output.WriteLine("Clearing all configuration locks");
                output.WriteLine("WARNING: This may allow changes to be lost.");
                revisionService.CleanupAllLocks();
                return 0;
            }

            public override string Suggest(string[] args)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}