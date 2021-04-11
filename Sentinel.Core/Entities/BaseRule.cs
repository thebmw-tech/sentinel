using System;
using Sentinel.Core.Enums;

namespace Sentinel.Core.Entities
{
    public class BaseRule<T> : BaseVersionedEntity<T> where T : BaseVersionedEntity<T>
    {
        public Guid Id { get; set; }
        public int Order { get; set; }
        public IPVersion IPVersion { get; set; }
        public IPProtocol Protocol { get; set; }

        public bool InvertSourceMatch { get; set; }
        public string SourceAddress { get; set; }
        public byte? SourceSubnetMask { get; set; }

        public ushort? SourcePortRangeStart { get; set; }
        public ushort? SourcePortRangeEnd { get; set; }

        public bool InvertDestinationMatch { get; set; }
        public string DestinationAddress { get; set; }
        public byte? DestinationSubnetMask { get; set; }

        public ushort? DestinationPortRangeStart { get; set; }
        public ushort? DestinationPortRangeEnd { get; set; }


        public bool Log { get; set; }
        public string Description { get; set; }
    }
}