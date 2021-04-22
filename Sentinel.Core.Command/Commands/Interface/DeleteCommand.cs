using Sentinel.Core.Command.Attributes;
using Sentinel.Core.Command.Enums;
using System.IO;
using Sentinel.Core.Command.Interfaces;
using Sentinel.Core.Command.Services;

namespace Sentinel.Core.Command.Commands.Interface
{
    [Command(CommandMode.Interface, "delete", "Delete some interface configuration")]
    public partial class DeleteCommand : BaseCommand
    {
        private readonly SubCommandInterpreter<DeleteCommand> subCommandInterpreter;

        public DeleteCommand(IShell shell, SubCommandInterpreter<DeleteCommand> subCommandInterpreter) : base(shell)
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

        public override void Help(string[] args, TextWriter output)
        {
            subCommandInterpreter.Help(shell, args);
        }

    }
}