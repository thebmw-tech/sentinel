using Sentinel.Core.Command.Attributes;
using Sentinel.Core.Command.Enums;
using Sentinel.Core.Command.Interfaces;
using System.IO;
using Sentinel.Core.Command.Services;

namespace Sentinel.Core.Command.Commands.Configuration
{
    [Command(CommandMode.Configuration, "set", "Set a configuration value")]
    public partial class SetCommand : ParentCommand<SetCommand>
    {
        public SetCommand(IShell shell, SubCommandInterpreter<SetCommand> subCommandInterpreter) : base(shell,
            subCommandInterpreter)
        {

        }
    }
}