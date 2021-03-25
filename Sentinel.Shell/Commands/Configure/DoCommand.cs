using Sentinel.Shell.Attributes;
using Sentinel.Shell.Enums;
using Sentinel.Shell.Helpers;
using Sentinel.Shell.Interfaces;
using Sentinel.Shell.Models;
using Sentinel.Shell.Services;

namespace Sentinel.Shell.Commands.Configure
{
    [Command(CommandMode.Configuration, "do", "Run a shell level command.")]
    public class DoCommand : ICommand
    {
        private readonly CommandInterpreter commandInterpreter;

        public DoCommand(CommandInterpreter commandInterpreter)
        {
            this.commandInterpreter = commandInterpreter;
        }

        public CommandReturn Execute(ShellContext context, string command)
        {
            var commandStr = HelperFunctions.GetSubCommand(command);
            commandInterpreter.Execute(CommandMode.Shell, context, commandStr);
            return CommandReturn.Normal;
        }

        public void Help(string command)
        {
            var commandStr = HelperFunctions.GetSubCommand(command);
            commandInterpreter.Help(CommandMode.Shell, commandStr);
        }

        public string Suggest(string command)
        {
            var commandStr = HelperFunctions.GetSubCommand(command);
            var suggestion = commandInterpreter.Suggest(CommandMode.Shell, commandStr);
            return $"do {suggestion}";
        } 

        
    }
}