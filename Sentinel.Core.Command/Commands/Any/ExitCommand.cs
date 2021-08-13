using Sentinel.Core.Command.Attributes;
using Sentinel.Core.Command.Enums;
using System.IO;
using Sentinel.Core.Command.Interfaces;
using Sentinel.Core.Factories;

namespace Sentinel.Core.Command.Commands.Any
{
    [Command(CommandMode.Any, "exit", "Exit current mode")]
    public class ExitCommand : BaseCommand
    {
        private readonly IEnvironmentSetup currentEnvironmentSetup;

        public ExitCommand(IShell shell, EnvironmentSetupFactory environmentSetupFactory) : base(shell)
        {
            currentEnvironmentSetup = environmentSetupFactory.Build(shell.CommandMode);
        }

        public override int Main(string[] args, TextReader input, TextWriter output, TextWriter error)
        {
            currentEnvironmentSetup.Cleanup(shell);

            return 0;
        }

        public override string Suggest(string[] args)
        {
            return "";
        }
    }
}