using System;
using Sentinel.Core.Helpers;
using Sentinel.Core.Models;
using Sentinel.Core.Services;
using Sentinel.Test.Helpers;
using Xunit;

namespace Sentinel.Test.Core.Services
{
    public class KernelInterfaceServiceTest
    {
        private readonly MockTestDIHelper<KernelInterfaceService> diHelper;

        private readonly KernelInterfaceService service;

        private const string IP_LINK_RESP =
            @"1: lo: <LOOPBACK,UP,LOWER_UP> mtu 65536 qdisc noqueue state UNKNOWN qlen 1000
    link/loopback 00:00:00:00:00:00 brd 00:00:00:00:00:00
2: eth0: <BROADCAST,MULTICAST,UP,LOWER_UP> mtu 1500 qdisc pfifo_fast state UP qlen 1000
    link/ether 00:07:32:4e:6e:2a brd ff:ff:ff:ff:ff:ff
3: wlan0: <BROADCAST,MULTICAST> mtu 1500 qdisc noop state DOWN qlen 1000
    link/ether 00:07:32:4e:6e:29 brd ff:ff:ff:ff:ff:ff
4: eth0.8@eth0: <BROADCAST,MULTICAST,UP,LOWER_UP> mtu 1500 qdisc noqueue state UP qlen 1000
    link/ether 00:07:32:4e:6e:2a brd ff:ff:ff:ff:ff:ff
5: eth0.3@eth0: <BROADCAST,MULTICAST,UP,LOWER_UP> mtu 1500 qdisc noqueue state UP qlen 1000
    link/ether 00:07:32:4e:6e:2a brd ff:ff:ff:ff:ff:ff";

        public KernelInterfaceServiceTest()
        {
            diHelper = new MockTestDIHelper<KernelInterfaceService>();

            service = diHelper.GetInstance();
        }

        [Fact]
        public void GetPhysicalInterfaceNamesTest()
        {
            diHelper.GetMock<ICommandExecutionHelper>().Setup(s => s.Execute("ip", "link")).Returns(new CommandExecutionResult() { Output = IP_LINK_RESP });

            var interfaceNames = service.GetPhysicalInterfaceNames();

            Assert.Equal(2, interfaceNames.Count);
            Assert.Contains("eth0", interfaceNames);
            Assert.Contains("wlan0", interfaceNames);
        }
    }
}