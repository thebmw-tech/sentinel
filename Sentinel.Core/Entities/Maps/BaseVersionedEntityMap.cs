using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sentinel.Core.Entities.Maps
{
    public class BaseVersionedEntityMap<T> : BaseEntityMap<T> where T : BaseVersionedEntity<T>
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);

            builder.Property(t => t.RevisionId)
                .IsRequired()
                .ValueGeneratedNever();

            builder.Property(t => t.Enabled)
                .IsRequired();
        }
    }
}