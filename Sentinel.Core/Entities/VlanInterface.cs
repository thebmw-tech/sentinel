namespace Sentinel.Core.Entities
{
    public class VlanInterface : BaseVersionedEntity<VlanInterface>
    {
        public string InterfaceName { get; set; }

        public string ParentInterfaceName { get; set; }

        public ushort VlanId { get; set; }
    }
}