using System;
using System.IO;
using Sentinel.Core.Command.Attributes;
using Sentinel.Core.Command.Enums;
using Sentinel.Core.Command.Interfaces;
using Sentinel.Core.Command.Services;
using Sentinel.Core.Services.Interfaces;

namespace Sentinel.Core.Command.Commands.Shell
{
    [Command(CommandMode.Shell, "show", "Shows Configuration")]
    public partial class ShowCommand : ParentCommand<ShowCommand>
    {
        public ShowCommand(IShell shell, SubCommandInterpreter<ShowCommand> subCommandInterpreter) : base(shell, subCommandInterpreter)
        {
        }
    }
}