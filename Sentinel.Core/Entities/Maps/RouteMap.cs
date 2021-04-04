using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sentinel.Core.Entities.Maps
{
    public class RouteMap : BaseVersionedEntityMap<Route>
    {
        public override void Configure(EntityTypeBuilder<Route> builder)
        {
            base.Configure(builder);

            builder.HasKey(r => new {r.RevisionId, r.Address, r.SubnetMask});

            builder.Property(r => r.Address)
                .HasMaxLength(45)
                .IsRequired();

            builder.Property(r => r.SubnetMask)
                .IsRequired();

            builder.Property(r => r.Version)
                .IsRequired();

            builder.Property(r => r.Description)
                .HasMaxLength(255);
        }
    }
}