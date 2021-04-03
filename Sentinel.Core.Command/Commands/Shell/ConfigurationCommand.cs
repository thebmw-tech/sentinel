using Sentinel.Core.Command.Attributes;
using Sentinel.Core.Command.Enums;
using Sentinel.Core.Command.Interfaces;
using Sentinel.Core.Command.Models;

namespace Sentinel.Core.Command.Commands.Shell
{
    [Command(CommandMode.Shell, "configure", "Enters into configuration mode.")]
    public class ConfigurationCommand : ICommand
    {
        private readonly IShell shell;

        public ConfigurationCommand(IShell shell)
        {
            this.shell = shell;
        }

        public CommandReturn Execute(string command)
        {
            var revisionId = SetupConfigRevision();

            var configContext = new ShellContext
            {
                { "revisionId", revisionId }
            };

            shell.CommandMode = CommandMode.Configuration;
            shell.Context = configContext;

            return CommandReturn.Normal;
        }

        public void Help(string command)
        {
            throw new System.NotImplementedException();
        }

        public string Suggest(string command)
        {
            return "configure";
        }

        private int SetupConfigRevision()
        {
            // TODO actualy do something here
            return 0;
        }
    }
}