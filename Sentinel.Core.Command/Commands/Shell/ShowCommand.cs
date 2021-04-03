using Sentinel.Core.Command.Attributes;
using Sentinel.Core.Command.Enums;
using Sentinel.Core.Command.Interfaces;
using Sentinel.Core.Command.Services;
using Sentinel.Core.Helpers;

namespace Sentinel.Core.Command.Commands.Shell
{
    [Command(CommandMode.Shell, "show", "TBD")]
    public partial class ShowCommand : ICommand
    {
        private readonly SubCommandInterpreter<ShowCommand> subCommandInterpreter;
        private readonly IShell shell;

        public ShowCommand(SubCommandInterpreter<ShowCommand> subCommandInterpreter, IShell shell)
        {
            this.subCommandInterpreter = subCommandInterpreter;
            this.shell = shell;
        }

        public CommandReturn Execute(string command)
        {
            var commandStr = HelperFunctions.GetSubCommand(command);
            
            subCommandInterpreter.Execute(this, shell, commandStr);
            return CommandReturn.Normal;
        }

        public void Help(string command)
        {
            subCommandInterpreter.Help(shell, command);
        }

        public string Suggest(string command)
        {
            return subCommandInterpreter.Suggest(shell, command);
        }
    }
}