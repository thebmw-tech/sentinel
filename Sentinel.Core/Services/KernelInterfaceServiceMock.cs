using Sentinel.Core.Services.Interfaces;
using System.Collections.Generic;

namespace Sentinel.Core.Services
{
    public class KernelInterfaceServiceMock : IKernelInterfaceService
    {
        public List<string> GetPhysicalInterfaceNames()
        {
            return new List<string>() { "eth0", "eth1", "eth2" };
        }
    }
}