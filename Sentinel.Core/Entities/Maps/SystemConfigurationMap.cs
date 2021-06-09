using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sentinel.Core.Entities.Maps
{
    public class SystemConfigurationMap : BaseVersionedEntityMap<SystemConfiguration>
    {
        public override void Configure(EntityTypeBuilder<SystemConfiguration> builder)
        {
            base.Configure(builder);

            builder.ToTable("SystemConfigurations");

            builder.HasKey(sc => sc.RevisionId);

            builder.Property(sc => sc.Enabled)
                .HasDefaultValue(true);

            builder.Property(sc => sc.Hostname)
                .HasMaxLength(63)
                .IsRequired();

            builder.Property(sc => sc.DHCPHostname)
                .HasMaxLength(63);
        }
    }
}