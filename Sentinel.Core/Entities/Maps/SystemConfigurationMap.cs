using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sentinel.Core.Entities.Maps
{
    public class SystemConfigurationMap : BaseEntityMap<SystemConfiguration>
    {
        public override void Configure(EntityTypeBuilder<SystemConfiguration> builder)
        {
            base.Configure(builder);
            builder.HasKey(sc => sc.RevisionId);


        }
    }
}