using System;
using Sentinel.Core.Services;
using Sentinel.Core.Services.Interfaces;
using Sentinel.Shell.Attributes;
using Sentinel.Shell.Enums;
using Sentinel.Shell.Interfaces;
using Sentinel.Shell.Models;

namespace Sentinel.Shell.Commands.Shell
{
    [Command(CommandMode.Shell, "configure", "Enters into configuration mode.")]
    public class ConfigurationCommand : ICommand
    {
        private readonly Services.Shell shell;

        public ConfigurationCommand(Services.Shell shell)
        {
            this.shell = shell;
        }

        public CommandReturn Execute(ShellContext context, string command)
        {
            var revisionId = SetupConfigRevision();

            var configContext = new ShellContext();
            configContext.Add("revisionId", revisionId);

            var getPrompt = new Func<string>(() =>
            {
                var prompt = "hostname(config)# ";
                return prompt;
            });

            shell.ShellLoop(CommandMode.Configuration, configContext, getPrompt);

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