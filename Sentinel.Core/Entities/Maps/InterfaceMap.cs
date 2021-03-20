using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sentinel.Core.Entities.Maps
{
    public class InterfaceMap : BaseEntityMap<Interface>
    {
        public override void Configure(EntityTypeBuilder<Interface> builder)
        {
            base.Configure(builder);

            builder.HasKey(i => new {i.RevisionId, i.Name});

            builder.Property(i => i.Description)
                .IsRequired()
                .HasMaxLength(255);

            builder
                .HasOne(i => i.IPv4Gateway)
                .WithOne(g => g.Interface);

            builder
                .HasOne(i => i.IPv6Gateway)
                .WithOne(g => g.Interface);
        }
    }
}