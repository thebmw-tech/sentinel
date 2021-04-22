using System.IO;
using Sentinel.Core.Command.Attributes;
using Sentinel.Core.Command.Interfaces;
using Sentinel.Core.Enums;
using Sentinel.Core.Helpers;
using Sentinel.Core.Repository.Interfaces;

namespace Sentinel.Core.Command.Commands.Interface
{
    public partial class DeleteCommand
    {
        [SubCommand("address", "")]
        public class DeleteAddressCommand : BaseCommand, IFilteredCommand
        {
            private readonly IInterfaceAddressRepository interfaceAddressRepository;
            public DeleteAddressCommand(IShell shell, IInterfaceAddressRepository interfaceAddressRepository) : base(shell)
            {
                this.interfaceAddressRepository = interfaceAddressRepository;
            }

            public override int Main(string[] args, TextReader input, TextWriter output, TextWriter error)
            {
                if (args.Length != 1)
                {
                    error.WriteLine("Wrong number of arguments");
                    return 1;
                }

                var addressString = args[0];

                // TODO validate incoming value
                var revisionId = shell.GetEnvironment<int>("CONFIG_REVISION_ID");
                var interfaceName = shell.GetEnvironment<string>("CONFIG_INTERFACE_NAME");

                if (addressString.Contains("dhcp"))
                {
                    if (addressString == "dhcp")
                    {
                        interfaceAddressRepository.Delete(a =>
                            a.RevisionId == revisionId && a.InterfaceName == interfaceName &&
                            a.AddressConfigurationType == AddressConfigurationType.DHCP);
                        interfaceAddressRepository.SaveChanges();
                    }
                    else if (addressString == "dhcp6")
                    {
                        interfaceAddressRepository.Delete(a =>
                            a.RevisionId == revisionId && a.InterfaceName == interfaceName &&
                            a.AddressConfigurationType == AddressConfigurationType.DHCP6);
                        interfaceAddressRepository.SaveChanges();
                    }
                }
                else
                {
                    var addressWithMask = AddressHelper.ValidateAndSplitAddressWithMask(addressString);

                    interfaceAddressRepository.Delete(a =>
                        a.RevisionId == revisionId && a.InterfaceName == interfaceName &&
                        a.AddressConfigurationType == AddressConfigurationType.Static &&
                        a.Address == addressWithMask.Item1 && a.SubnetMask == addressWithMask.Item2);
                    interfaceAddressRepository.SaveChanges();
                }

                output.WriteLine($"Removed address {addressString}");

                return 0;
            }

            public bool ShouldShow(IShell shell)
            {
                var interfaceType = shell.GetEnvironment<InterfaceType>("CONFIG_INTERFACE_TYPE");
                return interfaceType == InterfaceType.Ethernet || interfaceType == InterfaceType.Vlan ||
                       interfaceType == InterfaceType.Wireless;
            }

            public override string Suggest(string[] args)
            {
                return "";
            }
        }
    }
}