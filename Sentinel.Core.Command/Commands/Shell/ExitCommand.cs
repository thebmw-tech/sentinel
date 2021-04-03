using Sentinel.Core.Command.Attributes;
using Sentinel.Core.Command.Enums;
using Sentinel.Core.Command.Interfaces;

namespace Sentinel.Core.Command.Commands.Shell
{
    [Command(CommandMode.Shell, "exit", "Exits the shell.")]
    public class ExitCommand : ICommand
    {
        public CommandReturn Execute(string command)
        {
            return CommandReturn.Exit;
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