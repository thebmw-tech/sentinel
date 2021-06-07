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
            IFileSystem fileSystem, ILogger<NetworkDConfigurationGenerator> logger)
        {
            this.interfaceRepository = interfaceRepository;
            this.interfaceAddressRepository = interfaceAddressRepository;
            this.vlanInterfaceRepository = vlanInterfaceRepository;

            this.fileSystem = fileSystem;

            this.logger = logger;
        }

        public void Apply()
        {
            throw new System.NotImplementedException();
        }

        public void Generate()
        {
            throw new System.NotImplementedException();
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

            fileSystem.File.WriteAllText($"{NetworkFolder}{Path.PathSeparator}{virtualInterface.Name}.netdev", sb.ToString());
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

            fileSystem.File.WriteAllText($"{NetworkFolder}{Path.PathSeparator}{@interface.Name}.network", sb.ToString());
        }

        private void GenerateNetworkNetwork(Interface @interface, StringBuilder sb)
        {
            var addresses = interfaceAddressRepository.GetCurrent().Where(a => a.InterfaceName == @interface.Name);

            sb.Append("[Network]\n");
            sb.Append($"Description={@interface.Description}\n");


        }
    }
}