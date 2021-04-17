using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using Moq;
using Sentinel.Core.Entities;
using Sentinel.Core.Enums;
using Sentinel.Core.Generators.Netplan;
using Sentinel.Core.Helpers;
using Sentinel.Core.Models;
using Sentinel.Core.Repository.Interfaces;
using Sentinel.Test.Helpers;
using Xunit;
using Xunit.Abstractions;

namespace Sentinel.Test.Core.Generators.Netplan
{
    public class NetplanInterfaceConfigurationGeneratorTest
    {
        private readonly ITestOutputHelper outputHelper;

        private readonly MockTestDIHelper<NetplanInterfaceConfigurationGenerator> diHelper;

        private NetplanInterfaceConfigurationGenerator generator;

        public NetplanInterfaceConfigurationGeneratorTest(ITestOutputHelper outputHelper)
        {
            this.outputHelper = outputHelper;

            diHelper = new MockTestDIHelper<NetplanInterfaceConfigurationGenerator>();

            generator = diHelper.GetInstance();
        }

        [Fact]
        public void TestApply()
        {
            diHelper.GetMock<ICommandExecutionHelper>().Setup(s => s.Execute("netplan", "apply"))
                .Returns(new CommandExecutionResult() { ExitCode = 0 });

            generator.Apply();

            diHelper.GetMock<ICommandExecutionHelper>().Verify(v => v.Execute("netplan", "apply"), Times.Exactly(1));
        }

        [Fact]
        public void TestApplyFailure()
        {
            diHelper.GetMock<ICommandExecutionHelper>().Setup(s => s.Execute("netplan", "apply"))
                .Returns(new CommandExecutionResult() { ExitCode = 1, Output = "FailOut", Error = "FailError"});

            Assert.ThrowsAny<Exception>(() =>
            {
                generator.Apply();
            });

            diHelper.GetMock<ICommandExecutionHelper>().Verify(v => v.Execute("netplan", "apply"), Times.Exactly(1));
        }


        [Fact]
        public void TestGenerate()
        {
            diHelper.GetMock<IFileSystem>().Setup(s => s.File.WriteAllText(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((p, c) => { outputHelper.WriteLine(c); });

            diHelper.GetMock<ISystemConfigurationRepository>().Setup(s => s.GetCurrent())
                .Returns(new SystemConfiguration());

            
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