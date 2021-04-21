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
using Sentinel.Core.Helpers;
using Sentinel.Core.Repository.Interfaces;
using Sentinel.Models;

namespace Sentinel.Core.Command.Commands.Interface
{
    public partial class SetCommand
    {
        [SubCommand("address", "Adds and address to the interface")]
        public class SetAddressCommand : BaseCommand, IFilteredCommand
        {
            private readonly IInterfaceAddressRepository interfaceAddressRepository;
            public SetAddressCommand(IShell shell, IInterfaceAddressRepository interfaceAddressRepository) : base(shell)
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

                var revisionId = shell.GetEnvironment<int>("CONFIG_REVISION_ID");
                var interfaceName = shell.GetEnvironment<string>("CONFIG_INTERFACE_NAME");

                var address = new InterfaceAddress()
                {
                    RevisionId = revisionId,
                    InterfaceName = interfaceName
                };

                if (addressString.StartsWith("dhcp"))
                {
                    if (addressString == "dhcp")
                    {
                        var dhcpAddress = interfaceAddressRepository.Find(a =>
                            a.RevisionId == revisionId && a.InterfaceName == interfaceName &&
                            a.AddressConfigurationType == AddressConfigurationType.DHCP);
                        if (dhcpAddress != null)
                        {
                            error.WriteLine($"DHCP already configured for {interfaceName}");
                            return 1;
                        }
                        address.AddressConfigurationType = AddressConfigurationType.DHCP;
                    }
                    else
                    {
                        var dhcpAddress = interfaceAddressRepository.Find(a =>
                            a.RevisionId == revisionId && a.InterfaceName == interfaceName &&
                            a.AddressConfigurationType == AddressConfigurationType.DHCP6);
                        if (dhcpAddress != null)
                        {
                            error.WriteLine($"DHCP6 already configured for {interfaceName}");
                            return 1;
                        }
                        address.AddressConfigurationType = AddressConfigurationType.DHCP6;
                    }
                }
                else
                {
                    var addressWithMask = AddressHelper.ValidateAndSplitAddressWithMask(addressString);

                    var staticAddress = interfaceAddressRepository.Find(a =>
                        a.RevisionId == revisionId && a.InterfaceName == interfaceName &&
                        a.AddressConfigurationType == AddressConfigurationType.Static &&
                        a.Address == addressWithMask.Item1);
                    if (staticAddress != null)
                    {
                        error.WriteLine($"Address {addressString} already configured on {interfaceName}");
                        return 1;
                    }

                    address.AddressConfigurationType = AddressConfigurationType.Static;
                    address.Address = addressWithMask.Item1;
                    address.SubnetMask = addressWithMask.Item2;
                }


                interfaceAddressRepository.Create(address);
                interfaceAddressRepository.SaveChanges();

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
        }
    }
}
