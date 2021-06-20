using Sentinel.Core.Command.Attributes;
using Sentinel.Core.Command.Enums;
using Sentinel.Core.Command.Interfaces;
using Sentinel.Core.Repository.Interfaces;

namespace Sentinel.Core.Command.Commands.Interface
{
    [Command(CommandMode.Interface, "interface", "interface", false)]
    public class InterfaceCommand : Configuration.InterfaceCommand
    {
        public InterfaceCommand(IShell shell, IInterfaceRepository interfaceRepository,
            IVlanInterfaceRepository vlanInterfaceRepository) : base(shell, interfaceRepository,
            vlanInterfaceRepository)
        {

        }
    }
}