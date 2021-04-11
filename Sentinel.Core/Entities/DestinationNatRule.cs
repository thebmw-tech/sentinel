namespace Sentinel.Core.Entities
{
    public class DestinationNatRule : BaseNatRule<DestinationNatRule>
    {
        public string InboundInterfaceName { get; set; }
    }
}