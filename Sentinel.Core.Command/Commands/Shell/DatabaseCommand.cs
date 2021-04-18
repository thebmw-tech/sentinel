using Sentinel.Core.Command.Attributes;
using Sentinel.Core.Command.Enums;
using Sentinel.Core.Command.Interfaces;
using System.IO;
using Sentinel.Core.Command.Services;

namespace Sentinel.Core.Command.Commands.Shell
{
    [Command(CommandMode.Shell, "database", "Database Commands")]
    public partial class DatabaseCommand : BaseCommand
    {
        private readonly SubCommandInterpreter<DatabaseCommand> subCommandInterpreter;

        public DatabaseCommand(IShell shell, SubCommandInterpreter<DatabaseCommand> subCommandInterpreter) : base(shell)
        {
            this.subCommandInterpreter = subCommandInterpreter;
        }

        public override int Main(string[] args, TextReader input, TextWriter output, TextWriter error)
        {
            return subCommandInterpreter.Execute(shell, args, input, output, error);
        }

        public override string Suggest(string[] args)
        {
            return subCommandInterpreter.Suggest(shell, args);
        }
    }
}