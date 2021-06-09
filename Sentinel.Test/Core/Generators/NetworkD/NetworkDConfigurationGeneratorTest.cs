using System.Collections.Generic;
using Moq;
using NLog.Time;
using Sentinel.Core.Generators.IPTables;
using Sentinel.Test.Helpers;
using System.IO.Abstractions;
using System.Linq;
using Sentinel.Core.Entities;
using Sentinel.Core.Enums;
using Sentinel.Core.Generators.NetworkD;
using Sentinel.Core.Repository.Interfaces;
using Xunit;
using Xunit.Abstractions;

namespace Sentinel.Test.Core.Generators.NetworkD
{
    public class NetworkDConfigurationGeneratorTest
    {
        private readonly MockTestDIHelper<NetworkDConfigurationGenerator> diHelper;

        private readonly ITestOutputHelper outputHelper;

        private readonly NetworkDConfigurationGenerator generator;

        public NetworkDConfigurationGeneratorTest(ITestOutputHelper outputHelper)
        {
            this.outputHelper = outputHelper;

            diHelper = new MockTestDIHelper<NetworkDConfigurationGenerator>();

            generator = diHelper.GetInstance();
        }

        [Fact]
        public void TestGenerateSimple()
        {
            diHelper.GetMock<IFileSystem>().Setup(s => s.File.WriteAllText(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((p, c) =>
                {
                    outputHelper.WriteLine($"Writing contents of '{p}'");
                    outputHelper.WriteLine(c);


                });

            var interfaces = new List<Interface>();

            var wanInterface = new Interface()
            {
                Enabled = true,
                Name = "eth0",
                Description = "Wan",
                InterfaceType = InterfaceType.Ethernet
            };

            interfaces.Add(wanInterface);

            var lanInterface = new Interface()
            {
                Enabled = true,
                Name = "eth1",
                Description = "Lan",
                InterfaceType = InterfaceType.Ethernet
            };

            interfaces.Add(lanInterface);

            diHelper.GetMock<IInterfaceRepository>().Setup(s => s.GetCurrent()).Returns(interfaces.AsQueryable);


            var addresses = new List<InterfaceAddress>()
            {
                new InterfaceAddress
                {
                    InterfaceName = wanInterface.Name,
                    AddressConfigurationType = AddressConfigurationType.DHCP
                },
                new InterfaceAddress
                {
                    InterfaceName = lanInterface.Name,
                    AddressConfigurationType = AddressConfigurationType.Static,
                    Address = "192.168.1.1",
                    SubnetMask = 24
                }
            };

            diHelper.GetMock<IInterfaceAddressRepository>().Setup(s => s.GetCurrent()).Returns(addresses.AsQueryable);

            generator.Generate();
        }

        [Fact]
        public void TestGenerateStaticWan()
        {
            diHelper.GetMock<IFileSystem>().Setup(s => s.File.WriteAllText(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((p, c) =>
                {
                    outputHelper.WriteLine($"Writing contents of '{p}'");
                    outputHelper.WriteLine(c);


                });

            var interfaces = new List<Interface>();

            var wanInterface = new Interface()
            {
                Enabled = true,
                Name = "eth0",
                Description = "Wan",
                InterfaceType = InterfaceType.Ethernet
            };

            interfaces.Add(wanInterface);

            var lanInterface = new Interface()
            {
                Enabled = true,
                Name = "eth1",
                Description = "Lan",
                InterfaceType = InterfaceType.Ethernet
            };

            interfaces.Add(lanInterface);

            diHelper.GetMock<IInterfaceRepository>().Setup(s => s.GetCurrent()).Returns(interfaces.AsQueryable);


            var addresses = new List<InterfaceAddress>()
            {
                new InterfaceAddress
                {
                    InterfaceName = wanInterface.Name,
                    AddressConfigurationType = AddressConfigurationType.Static,
                    Address = "1.2.3.4",
                    SubnetMask = 22
                },
                new InterfaceAddress
                {
                    InterfaceName = lanInterface.Name,
                    AddressConfigurationType = AddressConfigurationType.Static,
                    Address = "192.168.1.1",
                    SubnetMask = 24
                }
            };

            diHelper.GetMock<IInterfaceAddressRepository>().Setup(s => s.GetCurrent()).Returns(addresses.AsQueryable);

            var routes = new List<Route>()
            {
                new Route()
                {
                    InterfaceName = wanInterface.Name,
                    RouteType = RouteType.Static,
                    Address = "0.0.0.0",
                    SubnetMask = 0,
                    NextHopAddress = "1.2.3.1"
                }
            };

            diHelper.GetMock<IRouteRepository>().Setup(s => s.GetCurrent()).Returns(routes.AsQueryable);

            generator.Generate();
        }

        [Fact]
        public void TestGenerateVlanInside()
        {
            diHelper.GetMock<IFileSystem>().Setup(s => s.File.WriteAllText(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((p, c) =>
                {
                    outputHelper.WriteLine($"Writing contents of '{p}'");
                    outputHelper.WriteLine(c);


                });

            var interfaces = new List<Interface>();

            var wanInterface = new Interface()
            {
                Enabled = true,
                Name = "eth0",
                Description = "Wan",
                InterfaceType = InterfaceType.Ethernet
            };

            interfaces.Add(wanInterface);

            var lanInterface = new Interface()
            {
                Enabled = true,
                Name = "eth1",
                Description = "Lan",
                InterfaceType = InterfaceType.Ethernet
            };

            interfaces.Add(lanInterface);

            var trunkInterface = new Interface()
            {
                Enabled = true,
                Name = "eth2",
                Description = "Trunk",
                InterfaceType = InterfaceType.Ethernet
            };

            interfaces.Add(trunkInterface);

            var dmzInterface = new Interface()
            {
                Enabled = true,
                Name = "eth2.13",
                Description = "DMZ",
                InterfaceType = InterfaceType.Vlan
            };

            interfaces.Add(dmzInterface);

            diHelper.GetMock<IInterfaceRepository>().Setup(s => s.GetCurrent()).Returns(interfaces.AsQueryable);


            var vlanInterfaces = new List<VlanInterface>()
            {
                new VlanInterface()
                {
                    InterfaceName = dmzInterface.Name,
                    ParentInterfaceName = trunkInterface.Name,
                    VlanId = 13
                }
            };

            diHelper.GetMock<IVlanInterfaceRepository>().Setup(s => s.GetCurrent()).Returns(vlanInterfaces.AsQueryable);


            var addresses = new List<InterfaceAddress>()
            {
                new InterfaceAddress
                {
                    InterfaceName = wanInterface.Name,
                    AddressConfigurationType = AddressConfigurationType.Static,
                    Address = "1.2.3.4",
                    SubnetMask = 22
                },
                new InterfaceAddress
                {
                    InterfaceName = lanInterface.Name,
                    AddressConfigurationType = AddressConfigurationType.Static,
                    Address = "192.168.1.1",
                    SubnetMask = 24
                },
                new InterfaceAddress
                {
                    InterfaceName = dmzInterface.Name,
                    AddressConfigurationType = AddressConfigurationType.Static,
                    Address = "192.168.2.1",
                    SubnetMask = 24
                }
            };

            diHelper.GetMock<IInterfaceAddressRepository>().Setup(s => s.GetCurrent()).Returns(addresses.AsQueryable);

            var routes = new List<Route>()
            {
                new Route()
                {
                    InterfaceName = wanInterface.Name,
                    RouteType = RouteType.Static,
                    Address = "0.0.0.0",
                    SubnetMask = 0,
                    NextHopAddress = "1.2.3.1"
                }
            };

            diHelper.GetMock<IRouteRepository>().Setup(s => s.GetCurrent()).Returns(routes.AsQueryable);

            generator.Generate();
        }
    }
}