using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using static Sentinel.Core.SentinelConstants;

namespace Sentinel.Core.Entities.Maps
{
    public class RouteMap : BaseVersionedEntityMap<Route>
    {
        public override void Configure(EntityTypeBuilder<Route> builder)
        {
            base.Configure(builder);

            builder.ToTable("Routes");

            builder.HasKey(r => new {r.RevisionId, r.Address, r.SubnetMask});

            builder.Property(r => r.InterfaceName)
                .HasMaxLength(INTERFACE_NAME_LENGTH)
                .IsRequired();

            builder.Property(r => r.Address)
                .HasMaxLength(IP_ADDRESS_LENGTH)
                .IsRequired();

            builder.Property(r => r.SubnetMask)
                .IsRequired();

            builder.Property(r => r.Version)
                .IsRequired();

            builder.Property(r => r.NextHopAddress)
                .HasMaxLength(IP_ADDRESS_LENGTH);

            builder.Property(r => r.Description)
                .HasMaxLength(DESCRIPTION_LENGTH);
        }
    }
}