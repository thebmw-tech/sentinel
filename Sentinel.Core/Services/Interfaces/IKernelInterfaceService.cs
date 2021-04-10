using System.Collections.Generic;

namespace Sentinel.Core.Services.Interfaces
{
    public interface IKernelInterfaceService
    {
        List<string> GetPhysicalInterfaceNames();
    }
}