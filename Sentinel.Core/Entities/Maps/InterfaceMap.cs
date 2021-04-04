using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sentinel.Core.Enums;

namespace Sentinel.Core.Entities.Maps
{
    public class InterfaceMap : BaseVersionedEntityMap<Interface>
    {
        public override void Configure(EntityTypeBuilder<Interface> builder)
        {
            base.Configure(builder);

            // Setup Primary Key
            builder.HasKey(i => new {i.RevisionId, i.Name});


            // Setup Properties
            builder.Property(i => i.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(i => i.InterfaceType)
                .IsRequired();

            builder.Property(i => i.Description)
                .HasMaxLength(255);

            builder.Property(i => i.IPv4ConfigurationType)
                .IsRequired()
                .HasDefaultValue(IpConfigurationTypeV4.None);

            builder.Property(i => i.IPv4Address)
                .HasMaxLength(15);

            builder.Property(i => i.IPv6ConfigurationType)
                .IsRequired()
                .HasDefaultValue(IpConfigurationTypeV6.None);

            builder.Property(i => i.IPv6Address)
                .HasMaxLength(39);
        }
    }
}