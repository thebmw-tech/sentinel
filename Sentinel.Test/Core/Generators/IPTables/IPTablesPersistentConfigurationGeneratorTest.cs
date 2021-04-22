using System.Collections.Generic;
using Moq;
using NLog.Time;
using Sentinel.Core.Generators.IPTables;
using Sentinel.Test.Helpers;
using System.IO.Abstractions;
using System.Linq;
using Sentinel.Core.Entities;
using Sentinel.Core.Enums;
using Sentinel.Core.Repository.Interfaces;
using Xunit;
using Xunit.Abstractions;

namespace Sentinel.Test.Core.Generators.IPTables
{
    public class IPTablesPersistentConfigurationGeneratorTest
    {
        private readonly MockTestDIHelper<IPTablesPersistentConfigurationGenerator> diHelper;

        private readonly ITestOutputHelper outputHelper;

        private readonly IPTablesPersistentConfigurationGenerator generator;

        public IPTablesPersistentConfigurationGeneratorTest(ITestOutputHelper outputHelper)
        {
            this.outputHelper = outputHelper;

            diHelper = new MockTestDIHelper<IPTablesPersistentConfigurationGenerator>();

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

            var wanInTable = Builders.FirewallTableBuilder.Buid("WAN_IN");
            var wanLocalTable = Builders.FirewallTableBuilder.Buid("WAN_LOCAL");
            var tables = new List<FirewallTable>() {wanInTable, wanLocalTable};

            diHelper.GetMock<IFirewallTableRepository>().Setup(s => s.GetCurrent()).Returns(tables.AsQueryable());

            var wanInterface = Builders.InterfaceBuilder.Build("eth0", inFw: wanInTable.Id, localFw: wanLocalTable.Id);
            var lanInterface = Builders.InterfaceBuilder.Build("eth1");
            var interfaces = new List<Sentinel.Core.Entities.Interface>() { wanInterface, lanInterface };

            diHelper.GetMock<IInterfaceRepository>().Setup(s => s.GetCurrent()).Returns(interfaces.AsQueryable());

            var defaultOutboundNatRule = new SourceNatRule()
            {
                Enabled = true,
                OutboundInterfaceName = wanInterface.Name,
                Order = 0,
                IPVersion = IPVersion.v4,
                SourceAddress = "192.168.1.1",
                SourceSubnetMask = 24
            };

            var sourceNatRules = new List<SourceNatRule>() {defaultOutboundNatRule};

            diHelper.GetMock<ISourceNatRuleRepository>().Setup(s => s.GetCurrent())
                .Returns(sourceNatRules.AsQueryable());

            var webServerPortForwardRule = new DestinationNatRule()
            {
                Order = 0,
                Enabled = true,
                IPVersion = IPVersion.v4,
                Protocol = IPProtocol.TCP,
                InboundInterfaceName = wanInterface.Name,
                DestinationAddress = "1.2.3.4",
                DestinationSubnetMask = 32,
                DestinationPortRangeStart = 80,
                DestinationPortRangeEnd = 80,
                TranslationAddress = "192.168.1.80",
            };

            var dstNatRules = new List<DestinationNatRule>() {webServerPortForwardRule};

            diHelper.GetMock<IDestinationNatRuleRepository>().Setup(s => s.GetCurrent())
                .Returns(dstNatRules.AsQueryable());


            var fwRules = new List<FirewallRule>();

            fwRules.Add(new FirewallRule()
            {
                Order = 0,
                Enabled = true,
                Action = FirewallAction.Pass,
                IPVersion = IPVersion.Both,
                FirewallTableId = wanInTable.Id,
                State = FirewallState.Established | FirewallState.Related,
            });

            fwRules.Add(new FirewallRule()
            {
                Order = 1,
                Enabled = true,
                Action = FirewallAction.Block,
                IPVersion = IPVersion.Both,
                FirewallTableId = wanInTable.Id,
                State = FirewallState.Invalid,
            });

            fwRules.Add(new FirewallRule()
            {
                Order = 2,
                Enabled = true,
                Action = FirewallAction.Pass,
                IPVersion = IPVersion.v4,
                Protocol = IPProtocol.TCP,
                FirewallTableId = wanInTable.Id,
                State = FirewallState.New,
                DestinationPortRangeStart = 80,
                DestinationPortRangeEnd = 80,
                DestinationAddress = "192.168.1.80",
                DestinationSubnetMask = 32
            });

            fwRules.Add(new FirewallRule()
            {
                Order = 3,
                Enabled = true,
                Action = FirewallAction.Pass,
                IPVersion = IPVersion.v6,
                Protocol = IPProtocol.TCP,
                FirewallTableId = wanInTable.Id,
                State = FirewallState.New,
                DestinationPortRangeStart = 80,
                DestinationPortRangeEnd = 80,
                DestinationAddress = "af00:dead:beef:cafe::80",
                DestinationSubnetMask = 128
            });

            diHelper.GetMock<IFirewallRuleRepository>().Setup(s => s.GetCurrent()).Returns(fwRules.AsQueryable());

            

            generator.Generate();
        }
    }
}