using Sentinel.Core.Command.Attributes;
using Sentinel.Core.Command.Enums;
using Sentinel.Core.Command.Interfaces;

namespace Sentinel.Core.Command.Commands.Configure
{
    [Command(CommandMode.Configuration, "exit", "Exits configuration mode.")]
    public class ExitCommand : ICommand
    {
        private readonly IShell shell;

        public ExitCommand(IShell shell)
        {
            this.shell = shell;
        }

        public CommandReturn Execute(string command)
        {
            shell.CommandMode = CommandMode.Shell;
            return CommandReturn.Normal;
        }

        public void Help(string command)
        {
            
        }

        public string Suggest(string command)
        {
            return "exit";
        }
    }
}