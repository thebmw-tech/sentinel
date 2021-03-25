using System;
using System.Reflection;
using Sentinel.Core;
using Sentinel.Shell.Attributes;
using Sentinel.Shell.Enums;
using Sentinel.Shell.Helpers;
using Sentinel.Shell.Interfaces;
using Sentinel.Shell.Models;
using Sentinel.Shell.Services;

namespace Sentinel.Shell.Commands.Shell
{
    [Command(CommandMode.Shell, "show", "TBD")]
    public partial class ShowCommand : ICommand
    {
        private readonly SubCommandInterpreter<ShowCommand> subCommandInterpreter;

        public ShowCommand(SubCommandInterpreter<ShowCommand> subCommandInterpreter)
        {
            this.subCommandInterpreter = subCommandInterpreter;
        }

        public CommandReturn Execute(ShellContext context, string command)
        {
            var commandStr = HelperFunctions.GetSubCommand(command);
            
            subCommandInterpreter.Execute(this, context, commandStr);
            return CommandReturn.Normal;
        }

        public void Help(string command)
        {
            subCommandInterpreter.Help(command);
        }

        public string Suggest(string command)
        {
            return subCommandInterpreter.Suggest(command);
        }
    }
}