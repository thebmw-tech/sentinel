using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sentinel.Core.Entities.Maps
{
    public class GatewayMap : BaseVersionedEntityMap<Gateway>
    {
        public override void Configure(EntityTypeBuilder<Gateway> builder)
        {
            base.Configure(builder);

            // Setup Keys
            builder.HasKey(g => new {g.RevisionId, g.Id});


            // Setup Properties
            builder.Property(g => g.Description)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(g => g.GatewayType)
                .IsRequired();

            builder.Property(g => g.InterfaceName)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(g => g.IPAddress)
                .HasMaxLength(45)
                .IsRequired();

            builder.Property(g => g.IPVersion)
                .IsRequired();


            // Setup Indexes
            builder.HasIndex(g => g.InterfaceName)
                .IsUnique(false);
        }
    }
}