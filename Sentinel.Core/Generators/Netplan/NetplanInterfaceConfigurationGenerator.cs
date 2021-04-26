using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using Microsoft.Extensions.Logging;
using Sentinel.Core.Enums;
using Sentinel.Core.Generators.Interfaces;
using Sentinel.Core.Helpers;
using Sentinel.Core.Models.Configuration.Netplan;
using Sentinel.Core.Repository.Interfaces;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using Route = Sentinel.Core.Models.Configuration.Netplan.Route;

namespace Sentinel.Core.Generators.Netplan
{
    public class NetplanInterfaceConfigurationGenerator : IConfigurationGenerator<NetplanInterfaceConfigurationGenerator>
    {
        private readonly IInterfaceRepository interfaceRepository;
        private readonly IInterfaceAddressRepository interfaceAddressRepository;
        private readonly ISystemConfigurationRepository systemConfigurationRepository;
        private readonly IRouteRepository routeRepository;

        private readonly ICommandExecutionHelper commandExecutionHelper;

        private readonly IFileSystem fileSystem;

        private readonly ILogger<NetplanInterfaceConfigurationGenerator> logger;

        public NetplanInterfaceConfigurationGenerator(IInterfaceRepository interfaceRepository, IInterfaceAddressRepository interfaceAddressRepository,
            ISystemConfigurationRepository systemConfigurationRepository, IRouteRepository routeRepository, ICommandExecutionHelper commandExecutionHelper,
            IFileSystem fileSystem, ILogger<NetplanInterfaceConfigurationGenerator> logger)
        {
            this.interfaceRepository = interfaceRepository;
            this.interfaceAddressRepository = interfaceAddressRepository;
            this.systemConfigurationRepository = systemConfigurationRepository;
            this.routeRepository = routeRepository;
            this.commandExecutionHelper = commandExecutionHelper;
            this.fileSystem = fileSystem;

            this.logger = logger;
        }

        public void Apply()
        {
            var result = commandExecutionHelper.Execute("netplan", "apply");
            if (result.ExitCode != 0)
            {
                logger.LogError($"netplan apply failed with exit code: {result.ExitCode}");
                logger.LogError(result.Output);
                logger.LogError(result.Error);
                throw new Exception($"netplan apply failed with exit code: {result.ExitCode}");
            }
            logger.LogInformation("netplan apply successful");
        }

        public void Generate()
        {
            var netplan = GenerateNetplan();

            var serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull)
                .Build();

            var yaml = serializer.Serialize(netplan);

            fileSystem.File.WriteAllText("/etc/netplan/01-sentinel.yaml", yaml);
        }

        private Models.Configuration.Netplan.Netplan GenerateNetplan()
        {
            var interfaces = interfaceRepository.GetCurrent();

            var netplan = new Models.Configuration.Netplan.Netplan()
            {
                Network = new Network()
                {
                    Version = 2,
                    Renderer = "networkd",
                    Ethernets = interfaces.Any(i => i.InterfaceType == InterfaceType.Ethernet) ? new Dictionary<string, Ethernet>() : null,
                    Vlans = interfaces.Any(i => i.InterfaceType == InterfaceType.Vlan) ? new Dictionary<string, Vlan>() : null
                }
            };


            foreach (var iface in interfaces.Where(i => i.InterfaceType == InterfaceType.Ethernet && i.Enabled))
            {
                var eth = new Ethernet()
                {
                    Renderer = "networkd",
                    Addresses = new List<string>()
                };

                AddAddressToInterface(eth, iface.Name);

                AddRoutesToInterface(eth, iface.Name);

                netplan.Network.Ethernets.Add(iface.Name, eth);
            }

            foreach (var iface in interfaces.Where(i => i.InterfaceType == InterfaceType.Vlan && i.Enabled))
            {
                string[] ifaceParts = iface.Name.Split('.');
                string name = ifaceParts[0];
                int vlanId = int.Parse(ifaceParts[1]);

                var vlan = new Vlan()
                {
                    Link = name,
                    Id = vlanId,
                    Addresses = new List<string>()
                };

                AddAddressToInterface(vlan, iface.Name);

                AddRoutesToInterface(vlan, iface.Name);

                netplan.Network.Vlans.Add(iface.Name.Replace('.', '_'), vlan);
            }

            return netplan;
        }

        private void AddAddressToInterface(Interface @interface, string interfaceName)
        {
            var addresses = interfaceAddressRepository.GetCurrent().Where(a => a.InterfaceName == interfaceName);
            foreach (var address in addresses)
            {
                switch (address.AddressConfigurationType)
                {
                    case AddressConfigurationType.Static:
                        @interface.Addresses.Add($"{address.Address}/{address.SubnetMask}");
                        break;
                    case AddressConfigurationType.DHCP:
                        @interface.Dhcp4 = true;
                        break;
                    case AddressConfigurationType.DHCP6:
                        @interface.Dhcp6 = true;
                        break;
                }
            }
        }

        private void AddRoutesToInterface(Interface iface, string interfaceName)
        {
            var routesForInterface = routeRepository.GetCurrent().Where(r => r.InterfaceName == interfaceName);
            if (routesForInterface.Any())
            {
                iface.Routes = routesForInterface.Select(t => new Route()
                { To = $"{t.Address}/{t.SubnetMask}", Via = t.NextHopAddress }).ToList();
            }
        }
    }
}