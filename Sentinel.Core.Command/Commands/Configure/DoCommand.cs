using Sentinel.Core.Command.Attributes;
using Sentinel.Core.Command.Enums;
using Sentinel.Core.Command.Interfaces;
using Sentinel.Core.Command.Services;
using Sentinel.Core.Helpers;

namespace Sentinel.Core.Command.Commands.Configure
{
    [Command(CommandMode.Configuration, "do", "Run a shell level command.")]
    public class DoCommand : ICommand
    {
        private readonly CommandInterpreter commandInterpreter;
        private readonly IShell shell;

        public DoCommand(CommandInterpreter commandInterpreter, IShell shell)
        {
            this.commandInterpreter = commandInterpreter;
            this.shell = shell;

        }

        public CommandReturn Execute(string command)
        {
            var commandStr = HelperFunctions.GetSubCommand(command);
            commandInterpreter.Execute(shell, CommandMode.Shell, commandStr);
            return CommandReturn.Normal;
        }

        public void Help(string command)
        {
            var commandStr = HelperFunctions.GetSubCommand(command);
            commandInterpreter.Help(shell, CommandMode.Shell, commandStr);
        }

        public string Suggest(string command)
        {
            var commandStr = HelperFunctions.GetSubCommand(command);
            var suggestion = commandInterpreter.Suggest(shell, CommandMode.Shell, commandStr);
            return $"do {suggestion}";
        } 

        
    }
}