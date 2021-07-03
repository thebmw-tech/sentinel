using Sentinel.Core.Command.Attributes;
using Sentinel.Core.Command.Enums;
using Sentinel.Core.Command.Interfaces;
using System.IO;
using System.Linq;
using Sentinel.Core.Environments;

namespace Sentinel.Core.Command.Commands.Interface
{
    [Command(CommandMode.Interface, "exit", "Exit from interface configuration")]
    public class ExitCommand : BaseCommand
    {
        private readonly InterfaceEnvironment interfaceEnvironment;

        public ExitCommand(IShell shell, InterfaceEnvironment interfaceEnvironment) : base(shell)
        {
            this.interfaceEnvironment = interfaceEnvironment;
        }

        public override int Main(string[] args, TextReader input, TextWriter output, TextWriter error)
        {
            interfaceEnvironment.Cleanup(shell);
            return 0;
        }

        public override string Suggest(string[] args)
        {
            throw new System.NotImplementedException();
        }
    }
}