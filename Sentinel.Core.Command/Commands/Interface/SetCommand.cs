using Sentinel.Core.Command.Attributes;
using Sentinel.Core.Command.Enums;
using Sentinel.Core.Command.Interfaces;
using System.IO;
using Sentinel.Core.Command.Services;
using Sentinel.Core.Helpers;
using Sentinel.Models;

namespace Sentinel.Core.Command.Commands.Interface
{
    [Command(CommandMode.Interface, "set", "Sets a configuration on the interface")]
    public partial class SetCommand : BaseCommand
    {
        private readonly SubCommandInterpreter<SetCommand> subCommandInterpreter;

        public SetCommand(IShell shell, SubCommandInterpreter<SetCommand> subCommandInterpreter) : base(shell)
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