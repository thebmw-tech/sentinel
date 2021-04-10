using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sentinel.Core.Entities.Maps
{
    public class FirewallRuleMap : BaseVersionedEntityMap<FirewallRule>
    {
        public override void Configure(EntityTypeBuilder<FirewallRule> builder)
        {
            base.Configure(builder);

            builder.HasKey(r => new { r.RevisionId, r.Id });

            builder.Property(r => r.Order)
                .IsRequired();

            builder.Property(r => r.Action)
                .IsRequired();

            builder.Property(r => r.InterfaceName)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(r => r.IpVersion)
                .IsRequired();

            builder.Property(r => r.Protocol)
                .IsRequired();

            builder.Property(r => r.InvertSourceMatch)
                .IsRequired();

            builder.Property(r => r.SourceAddress)
                .HasMaxLength(45);

            builder.Property(r => r.InvertDestinationMatch)
                .IsRequired();

            builder.Property(r => r.DestinationAddress)
                .HasMaxLength(45);

            builder.Property(r => r.Log)
                .IsRequired();

            builder.Property(r => r.Description)
                .HasMaxLength(255);
        }
    }
}