using Microsoft.EntityFrameworkCore.Metadata.Builders;

using static Sentinel.Core.SentinelConstants;

namespace Sentinel.Core.Entities.Maps
{
    public class FirewallTableMap : BaseVersionedEntityMap<FirewallTable>
    {
        public override void Configure(EntityTypeBuilder<FirewallTable> builder)
        {
            base.Configure(builder);

            builder.HasKey(t => new { t.RevisionId, t.Id });

            builder.Property(t => t.DefaultAction)
                .IsRequired();

            builder.Property(t => t.Name)
                .HasMaxLength(28)
                .IsRequired();

            builder.Property(t => t.Description)
                .HasMaxLength(DESCRIPTION_LENGTH);

            builder.Property(t => t.DefaultLog)
                .IsRequired();
        }
    }
}