using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sentinel.Core.Enums;
using static Sentinel.Core.SentinelConstants;

namespace Sentinel.Core.Entities.Maps
{
    public class InterfaceAddressMap : BaseVersionedEntityMap<InterfaceAddress>
    {
        public override void Configure(EntityTypeBuilder<InterfaceAddress> builder)
        {
            base.Configure(builder);

            builder.ToTable("InterfaceAddresses");

            builder.HasKey(a => new {a.RevisionId, a.InterfaceName, a.AddressConfigurationType, a.Address});


            builder.Property(a => a.InterfaceName)
                .HasMaxLength(INTERFACE_NAME_LENGTH)
                .IsRequired();

            builder.Property(a => a.Address)
                .HasMaxLength(IP_ADDRESS_LENGTH);

            builder.Property(a => a.AddressConfigurationType)
                .IsRequired()
                .HasDefaultValue(AddressConfigurationType.Static)
                .ValueGeneratedNever();

        }
    }
}