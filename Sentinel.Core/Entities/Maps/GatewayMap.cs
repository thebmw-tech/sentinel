using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sentinel.Core.Entities.Maps
{
    public class GatewayMap : BaseEntityMap<Gateway>
    {
        public override void Configure(EntityTypeBuilder<Gateway> builder)
        {
            base.Configure(builder);

            builder.HasKey(g => new {g.Id, g.RevisionId});

            builder
                .HasOne(g => g.Interface)
                .WithOne();
        }
    }
}