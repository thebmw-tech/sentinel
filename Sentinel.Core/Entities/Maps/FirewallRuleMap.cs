using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sentinel.Core.Entities.Maps
{
    public class FirewallRuleMap : BaseVersionedEntityMap<FirewallRule>
    {
        public override void Configure(EntityTypeBuilder<FirewallRule> builder)
        {
            base.Configure(builder);

            builder.ToTable("FirewallRules");

            builder.Property(r => r.Action)
                .IsRequired();

            builder.Property(r => r.FirewallTableId)
                .IsRequired();

            builder.Property(r => r.State)
                .IsRequired();

            builder.HasIndex(r => new { r.FirewallTableId, r.Order })
                .IsUnique();
        }
    }
}