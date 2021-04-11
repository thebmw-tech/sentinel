namespace Sentinel.Core.Entities
{
    public class SourceNatRule : BaseNatRule<SourceNatRule>
    {
        public string OutboundInterfaceName { get; set; }
    }
}