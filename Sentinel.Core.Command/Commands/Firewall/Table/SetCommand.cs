using Sentinel.Core.Command.Attributes;
using Sentinel.Core.Command.Enums;
using Sentinel.Core.Command.Interfaces;
using Sentinel.Core.Command.Services;

namespace Sentinel.Core.Command.Commands.Firewall.Table
{
    [Command(CommandMode.FirewallTable, "set", "")]
    public partial class SetCommand : ParentCommand<SetCommand>
    {
        public SetCommand(IShell shell, SubCommandInterpreter<SetCommand> subCommandInterpreter) : base(shell,
            subCommandInterpreter)
        {

        }
    }
}