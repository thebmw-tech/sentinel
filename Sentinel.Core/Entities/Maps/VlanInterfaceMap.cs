using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using static Sentinel.Core.SentinelConstants;

namespace Sentinel.Core.Entities.Maps
{
    public class VlanInterfaceMap : BaseVersionedEntityMap<VlanInterface>
    {
        public override void Configure(EntityTypeBuilder<VlanInterface> builder)
        {
            base.Configure(builder);

            builder.ToTable("vlaninterfaces");

            builder.HasKey(v => new {v.RevisionId, v.InterfaceName});

            builder.Property(v => v.InterfaceName)
                .HasMaxLength(INTERFACE_NAME_LENGTH)
                .IsRequired();

            builder.Property(v => v.ParentInterfaceName)
                .HasMaxLength(INTERFACE_NAME_LENGTH)
                .IsRequired();

            builder.Property(v => v.VlanId)
                .IsRequired();
        }
    }
}