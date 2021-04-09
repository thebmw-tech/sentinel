using System.Collections.Generic;

namespace Sentinel.Core.Services.Interfaces
{
    public interface IPhysicalInterfaceService
    {
        List<string> GetPhysicalInterfaceNames();
    }
}