using Sentinel.Core.Command.Attributes;
using Sentinel.Core.Command.Enums;
using Sentinel.Core.Command.Interfaces;
using System.IO;
using Sentinel.Core.Command.Services;

namespace Sentinel.Core.Command.Commands.Configuration
{
    [Command(CommandMode.Configuration, "edit", "Enter into an edit mode")]
    public partial class EditCommand : ParentCommand<EditCommand>
    {
        public EditCommand(IShell shell, SubCommandInterpreter<EditCommand> subCommandInterpreter) : base(shell,
            subCommandInterpreter)
        {

        }
    }
}