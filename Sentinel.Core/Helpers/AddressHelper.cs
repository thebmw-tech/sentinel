using System;
using System.Net;
using Sentinel.Core.Enums;

namespace Sentinel.Core.Helpers
{
    public static class AddressHelper
    {
        public static string AddressToNetwork(string address, byte netmask)
        {
            var ip = IPAddress.Parse(address);
            var version = address.Contains(':') ? IPVersion.v6 : IPVersion.v4;
            var mask = new IPAddress(BytesFromNetmask(netmask, version));
            var networkAddress = GetNetworkAddress(ip, mask);

            return networkAddress.ToString();

        }

        public static string NetmaskToSubnet(byte netmask)
        {
            if (netmask > 32)
            {
                throw new ArgumentOutOfRangeException(nameof(netmask), "IPv6 Does not use IP Like Subnetmasks");
            }
            var mask = new IPAddress(BytesFromNetmask(netmask, IPVersion.v4));
            return mask.ToString();
        }

        private static byte[] BytesFromNetmask(byte netmask, IPVersion version)
        {
            var length = version == IPVersion.v4 ? 4 : 16;
            byte[] bytes = new byte[length];
            for (var i = 0; i < length; i++)
            {
                byte newByte = 0;
                for (var x = 0; x < 8; x++)
                {
                    newByte = (byte)(newByte << 1);
                    if (netmask > 0)
                    {
                        newByte++;
                        netmask--;
                    }
                }

                bytes[i] = newByte;
            }

            return bytes;
        }

        private static IPAddress GetNetworkAddress(IPAddress address, IPAddress subnetMask)
        {
            byte[] ipAdressBytes = address.GetAddressBytes();
            byte[] subnetMaskBytes = subnetMask.GetAddressBytes();

            if (ipAdressBytes.Length != subnetMaskBytes.Length)
                throw new ArgumentException("Lengths of IP address and subnet mask do not match.");

            byte[] broadcastAddress = new byte[ipAdressBytes.Length];
            for (int i = 0; i < broadcastAddress.Length; i++)
            {
                broadcastAddress[i] = (byte)(ipAdressBytes[i] & (subnetMaskBytes[i]));
            }
            return new IPAddress(broadcastAddress);
        }

    }
}