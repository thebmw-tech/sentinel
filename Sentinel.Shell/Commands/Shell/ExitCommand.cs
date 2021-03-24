using Sentinel.Shell.Attributes;
using Sentinel.Shell.Enums;
using Sentinel.Shell.Interfaces;

namespace Sentinel.Shell.Commands.Shell
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