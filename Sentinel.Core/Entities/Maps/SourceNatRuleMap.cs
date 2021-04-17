using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using static Sentinel.Core.SentinelConstants;

namespace Sentinel.Core.Entities.Maps
{
    public class SourceNatRuleMap : BaseNatRuleMap<SourceNatRule>
    {
        public override void Configure(EntityTypeBuilder<SourceNatRule> builder)
        {
            base.Configure(builder);

            builder.ToTable("SourceNatRules");

            builder.Property(r => r.OutboundInterfaceName)
                .HasMaxLength(INTERFACE_NAME_LENGTH);
        }
    }
}