using Sentinel.Shell.Attributes;
using Sentinel.Shell.Enums;
using Sentinel.Shell.Interfaces;
using Sentinel.Shell.Models;

namespace Sentinel.Shell.Commands.Configure
{
    [Command(CommandMode.Configuration, "exit", "Exits configuration mode.")]
    public class ExitCommand : ICommand
    {
        public CommandReturn Execute(ShellContext context, string command)
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