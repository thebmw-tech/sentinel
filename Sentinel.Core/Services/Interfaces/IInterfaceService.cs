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

        bool InterfaceHasVlan(int revisionId, string interfaceName, ushort vlanId);

        void RemoveInterface(int revisionId, string name);
        void RemoveVlan(int revisionId, string parentInterfaceName, ushort vlandId);

        void PrintInterfaceToTextWriter(int revisionId, InterfaceDTO @interface, TextWriter writer);
    }
}