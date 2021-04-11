using Microsoft.EntityFrameworkCore.Metadata.Builders;

using static Sentinel.Core.SentinelConstants;

namespace Sentinel.Core.Entities.Maps
{
    public class DestinationNatRuleMap : BaseNatRuleMap<DestinationNatRule>
    {
        public override void Configure(EntityTypeBuilder<DestinationNatRule> builder)
        {
            base.Configure(builder);

            builder.Property(r => r.InboundInterfaceName)
                .HasMaxLength(INTERFACE_NAME_LENGTH);
        }
    }
}