using Sentinel.Core.Command.Attributes;
using Sentinel.Core.Command.Enums;
using Sentinel.Core.Command.Interfaces;
using Sentinel.Core.Command.Services;

namespace Sentinel.Core.Command.Commands.Configuration
{
    [Command(CommandMode.Configuration, "delete", "Delete a configuration entry")]
    public partial class DeleteCommand : ParentCommand<DeleteCommand>
    {
        public DeleteCommand(IShell shell, SubCommandInterpreter<DeleteCommand> subCommandInterpreter) : base(shell,
            subCommandInterpreter)
        {

        }
    }
}