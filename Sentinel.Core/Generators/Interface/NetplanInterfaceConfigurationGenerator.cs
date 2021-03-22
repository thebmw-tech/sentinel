using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using Sentinel.Core.Entities;
using Sentinel.Core.Enums;
using Sentinel.Core.Generators.Interfaces;
using Sentinel.Core.Models.Configuration.Netplan;
using Sentinel.Core.Services.Interfaces;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Sentinel.Core.Generators.Interface
{
    public class NetplanInterfaceConfigurationGenerator : IConfigurationGenerator<Entities.Interface>
    {
        private readonly IInterfaceService interfaceService;

        private readonly IFileSystem fileSystem;

        public NetplanInterfaceConfigurationGenerator(IInterfaceService interfaceService, IFileSystem fileSystem)
        {
            this.interfaceService = interfaceService;
            this.fileSystem = fileSystem;
        }

        public bool Apply()
        {
            throw new System.NotImplementedException();
        }

        public void Generate()
        {
            var netplan = GenerateNetplan();

            var serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull)
                .Build();

            var yaml = serializer.Serialize(netplan);

            fileSystem.File.WriteAllText("netplan", yaml);
        }

        private Netplan GenerateNetplan()
        {
            var interfaces = interfaceService.GetAllInterfacesCommitted();

            Netplan netplan = new Netplan()
            {
                Network = new Network()
                {
                    Version = 2,
                    Renderer = "networkd",
                    Ethernets = interfaces.Any(i => i.InterfaceType == InterfaceType.Ethernet) ? new Dictionary<string, Ethernet>() : null,
                    Vlans = interfaces.Any(i => i.InterfaceType == InterfaceType.Vlan) ? new Dictionary<string, Vlan>() : null
                }
            };


            foreach (var iface in interfaces.Where(i => i.InterfaceType == InterfaceType.Ethernet))
            {
                var eth = new Ethernet()
                {
                    Renderer = "networkd",
                    Dhcp4 = iface.IPv4ConfigurationType == IpConfigurationTypeV4.DHCP,
                    Addresses = new List<string>()
                };

                if (iface.IPv4ConfigurationType == IpConfigurationTypeV4.Static)
                {
                    eth.Addresses.Add($"{iface.IPv4Address}/{iface.IPv4SubnetMask}");
                }

                if (iface.IPv6ConfigurationType == IpConfigurationTypeV6.Static)
                {
                    eth.Addresses.Add($"{iface.IPv6Address}/{iface.IPv6SubnetMask}");
                }
                
                netplan.Network.Ethernets.Add(iface.Name, eth);
            }

            foreach (var iface in interfaces.Where(i => i.InterfaceType == InterfaceType.Vlan))
            {
                string[] ifaceParts = iface.Name.Split('.');
                string name = ifaceParts[0];
                int vlanId = int.Parse(ifaceParts[1]);

                var vlan = new Vlan()
                {
                    Link = name,
                    Id = vlanId,
                    Dhcp4 = iface.IPv4ConfigurationType == IpConfigurationTypeV4.DHCP,
                    Addresses = new List<string>()
                };

                if (iface.IPv4ConfigurationType == IpConfigurationTypeV4.Static)
                {
                    vlan.Addresses.Add($"{iface.IPv4Address}/{iface.IPv4SubnetMask}");
                }

                if (iface.IPv6ConfigurationType == IpConfigurationTypeV6.Static)
                {
                    vlan.Addresses.Add($"{iface.IPv6Address}/{iface.IPv6SubnetMask}");
                }

                netplan.Network.Vlans.Add(iface.Name.Replace('.', '_'), vlan);
            }

            return netplan;
        }

    }
}