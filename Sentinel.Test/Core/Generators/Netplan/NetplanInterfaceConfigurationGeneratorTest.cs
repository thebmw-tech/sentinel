using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using Moq;
using Sentinel.Core.Entities;
using Sentinel.Core.Enums;
using Sentinel.Core.Generators.Netplan;
using Sentinel.Core.Repository.Interfaces;
using Sentinel.Test.Helpers;
using Xunit;
using Xunit.Abstractions;

namespace Sentinel.Test.Core.Generators.Netplan
{
    public class NetplanInterfaceConfigurationGeneratorTest
    {
        private readonly ITestOutputHelper outputHelper;

        private readonly TestMockDIHelper<NetplanInterfaceConfigurationGenerator> diHelper;

        private NetplanInterfaceConfigurationGenerator generator;

        public NetplanInterfaceConfigurationGeneratorTest(ITestOutputHelper outputHelper)
        {
            this.outputHelper = outputHelper;

            diHelper = new TestMockDIHelper<NetplanInterfaceConfigurationGenerator>();

            generator = diHelper.GetInstance();
        }

        [Fact]
        public void TestGenerate()
        {
            diHelper.GetMock<IFileSystem>().Setup(s => s.File.WriteAllText(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((p, c) => { outputHelper.WriteLine(c); });

            diHelper.GetMock<ISystemConfigurationRepository>().Setup(s => s.GetCurrentConfiguration())
                .Returns(new SystemConfiguration());

            var testGateway = new Gateway()
            {
                Enabled = true,
                Id = Guid.NewGuid(),
                InterfaceName = "eth0",
                IPAddress = "192.168.1.254"
            };

            diHelper.GetMock<IGatewayRepository>().Setup(s => s.GetCurrentGatewayById(testGateway.Id))
                .Returns(testGateway);

            var testRoute = new Route()
            {
                Enabled = true,
                GatewayId = testGateway.Id,
                Address = "192.168.2.0",
                SubnetMask = 24
            };

            diHelper.GetMock<IRouteRepository>().Setup(s => s.GetCurrent()).Returns(new List<Route>() { testRoute }.AsQueryable());

            diHelper.GetMock<IInterfaceRepository>().Setup(s => s.GetCurrent())
                .Returns(
                    new List<Sentinel.Core.Entities.Interface>()
                    {
                        new Sentinel.Core.Entities.Interface()
                        {
                            Name = "eth0",
                            Enabled = true,
                            IPv4Address = "192.168.1.1",
                            IPv4SubnetMask = 24,
                            IPv4ConfigurationType = IpConfigurationTypeV4.Static,
                            InterfaceType = InterfaceType.Ethernet
                        },
                        new Sentinel.Core.Entities.Interface()
                        {
                            Name = "eth1.1337",
                            Enabled = true,
                            IPv4Address = "192.168.1.1",
                            IPv4SubnetMask = 24,
                            IPv4ConfigurationType = IpConfigurationTypeV4.Static,
                            InterfaceType = InterfaceType.Vlan
                        }
                    }.AsQueryable());

            generator.Generate();
        }
    }
}