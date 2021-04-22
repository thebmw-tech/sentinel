using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sentinel.Core.Enums;

using static Sentinel.Core.SentinelConstants;

namespace Sentinel.Core.Entities.Maps
{
    public class InterfaceMap : BaseVersionedEntityMap<Interface>
    {
        public override void Configure(EntityTypeBuilder<Interface> builder)
        {
            base.Configure(builder);

            builder.ToTable("Interfaces");

            // Setup Primary Key
            builder.HasKey(i => new {i.RevisionId, i.Name});


            // Setup Properties
            builder.Property(i => i.Name)
                .HasMaxLength(INTERFACE_NAME_LENGTH)
                .IsRequired();

            builder.Property(i => i.InterfaceType)
                .IsRequired();

            builder.Property(i => i.Description)
                .HasMaxLength(DESCRIPTION_LENGTH);
        }
    }
}