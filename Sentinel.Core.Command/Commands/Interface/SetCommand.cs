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
        private SubCommandInterpreter<SetCommand> subCommandInterpreter;

        public SetCommand(IShell shell, SubCommandInterpreter<SetCommand> subCommandInterpreter) : base(shell)
        {
            this.subCommandInterpreter = subCommandInterpreter;
        }

        public override int Main(string[] args, TextReader input, TextWriter output, TextWriter error)
        {
            return 0;
        }

        public override string Suggest(string[] args)
        {
            throw new System.NotImplementedException();
        }
    }
}