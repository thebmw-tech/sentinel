using Microsoft.EntityFrameworkCore.Metadata.Builders;

using static Sentinel.Core.SentinelConstants;

namespace Sentinel.Core.Entities.Maps
{
    public class BaseNatRuleMap<T> : BaseRuleMap<T> where T : BaseNatRule<T>
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);

            builder.Property(r => r.TranslationAddress)
                .HasMaxLength(IP_ADDRESS_LENGTH);

            builder.HasIndex(r => r.Order)
                .IsUnique();
        }
    }
}