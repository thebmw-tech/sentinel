using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sentinel.Core.Command.Attributes;
using Sentinel.Core.Command.Interfaces;
using Sentinel.Core.Entities;
using Sentinel.Core.Enums;
using Sentinel.Core.Repository.Interfaces;
using Sentinel.Models;

namespace Sentinel.Core.Command.Commands.Interface
{
    public partial class SetCommand
    {
        [SubCommand("vlan", "Sets the interface description")]
        public class SetParentInterfaceCommand : BaseCommand, IFilteredCommand
        {
            private readonly IInterfaceRepository interfaceRepository;
            private readonly IVlanInterfaceRepository vlanInterfaceRepository;

            public SetParentInterfaceCommand(IShell shell, IInterfaceRepository interfaceRepository, IVlanInterfaceRepository vlanInterfaceRepository) : base(shell)
            {
                this.interfaceRepository = interfaceRepository;
                this.vlanInterfaceRepository = vlanInterfaceRepository;
            }

            public override int Main(string[] args, TextReader input, TextWriter output, TextWriter error)
            {
                if (args.Length != 1)
                {
                    error.WriteLine("Wrong number of arguments");
                    return 1;
                }


                // TODO validate incoming value
                var revisionId = shell.GetEnvironment<int>("CONFIG_REVISION_ID");
                var interfaceName = shell.GetEnvironment<string>("CONFIG_INTERFACE_NAME");

                var vlanId = ushort.MaxValue;
                if (!ushort.TryParse(args[0], out vlanId))
                {
                    error.WriteLine("Invalid vlan id.");
                    return 2;
                }

                var vlanInterface = vlanInterfaceRepository.Find(v =>
                    v.RevisionId == revisionId && v.ParentInterfaceName == interfaceName && v.VlanId == vlanId);

                if (vlanInterface != null)
                {
                    error.WriteLine($"Vlan {vlanId} already configured on {interfaceName}");
                    return 3;
                }

                vlanInterface = new VlanInterface()
                {
                    RevisionId = revisionId,
                    ParentInterfaceName = interfaceName,
                    VlanId = vlanId,
                    InterfaceName = $"{interfaceName}.{vlanId}"
                };

                vlanInterfaceRepository.Create(vlanInterface);
                vlanInterfaceRepository.SaveChanges();


                return 0;
            }

            public override string Suggest(string[] args)
            {
                if (args.Length > 0)
                {
                    if (string.IsNullOrEmpty(args[0]))
                    {
                        return " ";
                    }
                    return args[0];
                }

                return " ";
            }

            public bool ShouldShow(IShell shell)
            {
                var interfaceType = shell.GetEnvironment<InterfaceType>("CONFIG_INTERFACE_TYPE");
                return interfaceType == InterfaceType.Ethernet;
            }
        }
    }
}
