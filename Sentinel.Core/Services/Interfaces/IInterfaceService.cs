using System.Collections.Generic;
using Sentinel.Core.Entities;
using Sentinel.Models;

namespace Sentinel.Core.Services.Interfaces
{
    public interface IInterfaceService
    {
        InterfaceDTO GetInterfaceWithName(int revisionId, string name);
    }
}