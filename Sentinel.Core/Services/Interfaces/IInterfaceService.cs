using System.Collections.Generic;
using System.IO;
using Sentinel.Core.Entities;
using Sentinel.Models;

namespace Sentinel.Core.Services.Interfaces
{
    public interface IInterfaceService
    {
        List<InterfaceDTO> GetInterfacesInRevision(int revisionId);
        InterfaceDTO GetInterfaceWithName(int revisionId, string name);

        void PrintInterfaceToTextWriter(int revisionId, InterfaceDTO @interface, TextWriter writer);
    }
}