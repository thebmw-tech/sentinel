using System;
using System.Collections.Generic;
using Sentinel.Core.Helpers;
using Xunit;

namespace Sentinel.Test.Core.Helpers
{
    public class AddressHelperTest
    {
        [Fact]
        public void TestAddressToNetwork()
        {
            // Tests are stored as tuples (Address, NetMask, Network)
            var tests = new List<Tuple<string, byte, string>>()
            {
                new Tuple<string, byte, string>("192.168.150.7", 24, "192.168.150.0"),
                new Tuple<string, byte, string>("192.168.150.7", 22, "192.168.148.0"),
                new Tuple<string, byte, string>("af00:dead:beef:cafe::1337", 64, "af00:dead:beef:cafe::"),
                new Tuple<string, byte, string>("af00:dead:beef:cafe::1337", 56, "af00:dead:beef:ca00::"),
                new Tuple<string, byte, string>("af00:dead:beef:cafe::1337", 48, "af00:dead:beef::")
            };

            foreach (var test in tests)
            {
                var networkAddress = AddressHelper.AddressToNetwork(test.Item1, test.Item2);
                Assert.Equal(test.Item3, networkAddress);
            }
        }
    }
}