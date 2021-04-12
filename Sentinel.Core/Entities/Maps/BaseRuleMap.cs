using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using static Sentinel.Core.SentinelConstants;

namespace Sentinel.Core.Entities.Maps
{
    public abstract class BaseRuleMap<T> : BaseVersionedEntityMap<T> where T : BaseRule<T>
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);

            builder.HasKey(r => new { r.RevisionId, r.Id });

            builder.Property(r => r.Order)
                .IsRequired();

            builder.Property(r => r.IPVersion)
                .IsRequired();

            builder.Property(r => r.Protocol)
                .IsRequired();

            builder.Property(r => r.InvertSourceMatch)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(r => r.SourceAddress)
                .HasMaxLength(IP_ADDRESS_LENGTH);

            builder.Property(r => r.InvertDestinationMatch)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(r => r.DestinationAddress)
                .HasMaxLength(IP_ADDRESS_LENGTH);

            builder.Property(r => r.Log)
                .IsRequired();

            builder.Property(r => r.Description)
                .HasMaxLength(255);


            
        }
    }
}