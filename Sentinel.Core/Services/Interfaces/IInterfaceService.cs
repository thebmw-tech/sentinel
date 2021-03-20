using System.Collections.Generic;
using Sentinel.Core.Entities;

namespace Sentinel.Core.Services.Interfaces
{
    public interface IInterfaceService
    {
        List<Interface> GetAllInterfacesCommitted();
    }
}