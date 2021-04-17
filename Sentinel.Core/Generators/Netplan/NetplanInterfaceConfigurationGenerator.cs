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
    public class NetplanInterfaceConfigurationGenerator : IConfigurationGenerator<Entities.Interface>
    {
        private readonly IInterfaceRepository interfaceRepository;
        private readonly ISystemConfigurationRepository systemConfigurationRepository;
        private readonly IRouteRepository routeRepository;

        private readonly ICommandExecutionHelper commandExecutionHelper;

        private readonly IFileSystem fileSystem;

        private readonly ILogger<NetplanInterfaceConfigurationGenerator> logger;

        public NetplanInterfaceConfigurationGenerator(IInterfaceRepository interfaceRepository, ISystemConfigurationRepository systemConfigurationRepository, 
            IRouteRepository routeRepository, ICommandExecutionHelper commandExecutionHelper, IFileSystem fileSystem, 
            ILogger<NetplanInterfaceConfigurationGenerator> logger)
        {
            this.interfaceRepository = interfaceRepository;
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

            var currentSystemConfiguration = systemConfigurationRepository.GetCurrent();

            //var currentRoutes = routeRepository.GetCurrent().Select(r =>
            //    new Tuple<Entities.Route, Entities.Gateway>(r, gatewayRepository.GetCurrentGatewayById(r.GatewayId))).ToList();

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
                    Dhcp4 = iface.IPv4ConfigurationType == IpConfigurationTypeV4.DHCP,
                    Dhcp6 = iface.IPv6ConfigurationType == IpConfigurationTypeV6.DHCP6,
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

                //AddRoutesToInterface(eth, iface.Name, currentRoutes);

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
                    Dhcp4 = iface.IPv4ConfigurationType == IpConfigurationTypeV4.DHCP,
                    Dhcp6 = iface.IPv6ConfigurationType == IpConfigurationTypeV6.DHCP6,
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

                //AddRoutesToInterface(vlan, iface.Name, currentRoutes);

                netplan.Network.Vlans.Add(iface.Name.Replace('.', '_'), vlan);
            }

            return netplan;
        }

        //private void AddRoutesToInterface(Models.Configuration.Netplan.Interface iface, string interfaceName,
        //    List<Tuple<Entities.Route, Entities.Gateway>> routes)
        //{
        //    var routesForInterface = routes.Where(r => r.Item2.InterfaceName == interfaceName).ToList();
        //    if (routesForInterface.Any())
        //    {
        //        iface.Routes = routesForInterface.Select(t => new Route()
        //            {To = $"{t.Item1.Address}/{t.Item1.SubnetMask}", Via = t.Item2.IPAddress}).ToList();
        //    }
        //}
    }
}