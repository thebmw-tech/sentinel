using Sentinel.Core.Enums;

namespace Sentinel.Core.Entities
{
    public class InterfaceAddress : BaseVersionedEntity<InterfaceAddress>
    {
        public string InterfaceName { get; set; }


        public AddressConfigurationType AddressConfigurationType { get; set; }
        public string Address { get; set; }
        public byte? SubnetMask { get; set; }
    }
}