using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using Moq;
using Sentinel.Core.Enums;
using Sentinel.Core.Generators.Interface;
using Sentinel.Core.Services.Interfaces;
using Xunit;

namespace Sentinel.Test.Core.Generators.Interface
{
    public class NetpalnInterfaceConfigurationGeneratorTest
    {
        private Mock<IFileSystem> filesystemMock;
        private Mock<IInterfaceService> interfaceServiceMock;

        private NetplanInterfaceConfigurationGenerator generator;

        public NetpalnInterfaceConfigurationGeneratorTest()
        {
            filesystemMock = new Mock<IFileSystem>();
            interfaceServiceMock = new Mock<IInterfaceService>();

            generator = new NetplanInterfaceConfigurationGenerator(interfaceServiceMock.Object, filesystemMock.Object);
        }

        [Fact]
        public void TestGenerate()
        {
            filesystemMock.Setup(s => s.File.WriteAllText(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((p, c) => { Console.WriteLine(c); });

            interfaceServiceMock.Setup(s => s.GetAllInterfacesCommitted())
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
                        }
                    });

            generator.Generate();
        }
    }
}