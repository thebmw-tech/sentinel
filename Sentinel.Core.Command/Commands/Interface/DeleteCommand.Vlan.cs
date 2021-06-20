using System.IO;
using Sentinel.Core.Command.Attributes;
using Sentinel.Core.Command.Interfaces;
using Sentinel.Core.Enums;
using Sentinel.Core.Helpers;
using Sentinel.Core.Repository.Interfaces;
using Sentinel.Core.Services.Interfaces;

namespace Sentinel.Core.Command.Commands.Interface
{
    public partial class DeleteCommand
    {
        [SubCommand("vlan", "")]
        public class DeleteVlanCommand : BaseCommand, IFilteredCommand
        {
            private readonly IInterfaceService interfaceService;
            public DeleteVlanCommand(IShell shell, IInterfaceService interfaceService) : base(shell)
            {
                this.interfaceService = interfaceService;
            }

            public override int Main(string[] args, TextReader input, TextWriter output, TextWriter error)
            {
                if (args.Length != 1)
                {
                    error.WriteLine("Wrong number of arguments");
                    return 1;
                }

                ushort vlanId;
                if (!ushort.TryParse(args[0], out vlanId))
                {
                    error.WriteLine("Invalid vlan id");
                    return 2;
                }

                

                return 0;
            }

            public bool ShouldShow(IShell shell)
            {
                var interfaceType = shell.GetEnvironment<InterfaceType>("CONFIG_INTERFACE_TYPE");
                return interfaceType == InterfaceType.Ethernet;
            }

            public override string Suggest(string[] args)
            {
                return "";
            }
        }
    }
}