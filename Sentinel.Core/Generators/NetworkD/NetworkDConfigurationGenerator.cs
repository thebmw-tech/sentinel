using System;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using Sentinel.Core.Entities;
using Sentinel.Core.Enums;
using Sentinel.Core.Generators.Interfaces;
using Sentinel.Core.Repository.Interfaces;

namespace Sentinel.Core.Generators.NetworkD
{
    public class NetworkDConfigurationGenerator : IConfigurationGenerator<NetworkDConfigurationGenerator>
    {
        private readonly IInterfaceRepository interfaceRepository;
        private readonly IInterfaceAddressRepository interfaceAddressRepository;
        private readonly IVlanInterfaceRepository vlanInterfaceRepository;
        private readonly IRouteRepository routeRepository;

        private readonly IFileSystem fileSystem;

        private readonly ILogger<NetworkDConfigurationGenerator> logger;

        private const string NetworkFolder = "/etc/systemd/network/";

        private readonly InterfaceType[] virtualInterfaceTypes = new InterfaceType[]
        {
            InterfaceType.Vlan,
            InterfaceType.Wireguard
        };

        public NetworkDConfigurationGenerator(IInterfaceRepository interfaceRepository,
            IInterfaceAddressRepository interfaceAddressRepository, IVlanInterfaceRepository vlanInterfaceRepository,
            IRouteRepository routeRepository, IFileSystem fileSystem, ILogger<NetworkDConfigurationGenerator> logger)
        {
            this.interfaceRepository = interfaceRepository;
            this.interfaceAddressRepository = interfaceAddressRepository;
            this.vlanInterfaceRepository = vlanInterfaceRepository;
            this.routeRepository = routeRepository;

            this.fileSystem = fileSystem;

            this.logger = logger;
        }

        public void Apply()
        {
            throw new System.NotImplementedException();
        }

        public void Generate()
        {
            CleanupFiles();
            GenerateNetDevs();
            GenerateNetworks();
        }

        private void CleanupFiles()
        {

        }

        private void GenerateNetDevs()
        {
            var virtualInterfaces = interfaceRepository.GetCurrent().Where(i => i.Enabled && virtualInterfaceTypes.Contains(i.InterfaceType));
            foreach (var virtualInterface in virtualInterfaces)
            {
                GenerateNetDev(virtualInterface);
            }
        }

        private void GenerateNetDev(Interface virtualInterface)
        {
            var sb = new StringBuilder();

            sb.Append("[NetDev]\n");
            sb.Append($"Name={virtualInterface.Name}\n");
            sb.Append($"Description={virtualInterface.Description}\n");
            sb.Append($"Kind={virtualInterface.InterfaceType.ToString().ToLower()}\n");
            sb.Append("\n");

            switch (virtualInterface.InterfaceType)
            {
                case InterfaceType.Vlan:
                    GenerateVlanNetDev(virtualInterface, sb);
                    break;
                default:
                    throw new Exception("We should never get here");
            }

            fileSystem.File.WriteAllText($"{NetworkFolder}{virtualInterface.Name}.netdev", sb.ToString());
        }

        private void GenerateVlanNetDev(Interface virtualInterface, StringBuilder sb)
        {
            var vlan = vlanInterfaceRepository.GetCurrent().First(v => v.InterfaceName == virtualInterface.Name);

            sb.Append("[VLAN]\n");
            sb.Append($"Id={vlan.VlanId}\n");
        }

        private void GenerateNetworks()
        {
            var interfaces = interfaceRepository.GetCurrent().Where(i => i.Enabled);
            foreach (var @interface in interfaces)
            {
                GenerateNetwork(@interface);
            }
        }

        private void GenerateNetwork(Interface @interface)
        {
            var sb = new StringBuilder();

            sb.Append("[Match]\n");
            sb.Append($"Name={@interface.Name}\n");
            sb.Append("\n");

            GenerateNetworkNetwork(@interface, sb);
            sb.Append("\n");
            GenerateNetworkRoutes(@interface, sb);

            fileSystem.File.WriteAllText($"{NetworkFolder}{@interface.Name}.network", sb.ToString());
        }

        private void GenerateNetworkNetwork(Interface @interface, StringBuilder sb)
        {
            var addresses = interfaceAddressRepository.GetCurrent().Where(a => a.InterfaceName == @interface.Name);

            sb.Append("[Network]\n");
            sb.Append($"Description={@interface.Description}\n");

            var dhcpMode = "no";

            if (addresses.Any(a =>
                a.AddressConfigurationType == AddressConfigurationType.DHCP ||
                a.AddressConfigurationType == AddressConfigurationType.DHCP6))
            {
                if (!addresses.Any(a => a.AddressConfigurationType == AddressConfigurationType.DHCP6))
                {
                    dhcpMode = "ipv4";
                }
                else if (!addresses.Any(a => a.AddressConfigurationType == AddressConfigurationType.DHCP))
                {
                    dhcpMode = "ipv6";
                }
                else
                {
                    dhcpMode = "yes";
                }
            }

            sb.Append($"DHCP={dhcpMode}\n");

            foreach (var address in addresses.Where(a => a.AddressConfigurationType == AddressConfigurationType.Static))
            {
                sb.Append($"Address={address.Address}/{address.SubnetMask}\n");
            }

            sb.Append("\n");

            GenerateVirtualNetworkNetwork(@interface, sb);
        }

        private void GenerateVirtualNetworkNetwork(Interface @interface, StringBuilder sb)
        {
            var vlanInterfaces = vlanInterfaceRepository.GetCurrent().Where(v => v.ParentInterfaceName == @interface.Name);
            foreach (var vlanInterface in vlanInterfaces)
            {
                sb.Append($"VLAN={vlanInterface.InterfaceName}\n");
            }
        }

        private void GenerateNetworkRoutes(Interface @interface, StringBuilder sb)
        {
            var interfaceRoutes = routeRepository.GetCurrent().Where(r => r.InterfaceName == @interface.Name);
            
            foreach (var interfaceRoute in interfaceRoutes)
            {
                sb.Append("[Route]\n");
                sb.Append($"Destination={interfaceRoute.Address}/{interfaceRoute.SubnetMask}\n");
                switch (interfaceRoute.RouteType)
                {
                    case RouteType.Static:
                        sb.Append($"Gateway={interfaceRoute.NextHopAddress}\n");
                        sb.Append("Type=unicast\n");
                        break;
                    case RouteType.Null:
                        sb.Append("Type=blackhole\n");
                        break;
                    default:
                        throw new NotImplementedException($"{interfaceRoute.RouteType} is not supported.");
                }
            }
        }
    }
}