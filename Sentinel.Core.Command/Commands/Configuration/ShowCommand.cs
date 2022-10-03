using Sentinel.Core.Command.Attributes;
using Sentinel.Core.Command.Enums;
using Sentinel.Core.Command.Interfaces;
using Sentinel.Core.Command.Services;

namespace Sentinel.Core.Command.Commands.Configuration;

[Command(CommandMode.Configuration, "show", "Shows the configuration")]
public partial class ShowCommand : ParentCommand<ShowCommand>
{
    public ShowCommand(IShell shell, SubCommandInterpreter<ShowCommand> subCommandInterpreter) : base(shell, subCommandInterpreter)
    {
    }
}