using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sentinel.Core.Entities.Maps
{
    public class RouteMap : BaseEntityMap<Route>
    {
        public override void Configure(EntityTypeBuilder<Route> builder)
        {
            base.Configure(builder);

            builder.HasKey(r => new {r.RevisionId, r.Address, r.SubnetMask});

            builder.HasOne(r => r.Gateway).WithOne().HasForeignKey<Route>(r => r.GatewayId);
        }
    }
}