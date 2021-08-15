using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sentinel.Core.Entities.Maps
{
    public class UserKeyMap : BaseEntityMap<UserKey>
    {
        public override void Configure(EntityTypeBuilder<UserKey> builder)
        {
            builder.ToTable("UserKeys");

            builder.HasKey(uk => uk.Id);

            builder.Property(uk => uk.UserId)
                .IsRequired();

            builder.Property(uk => uk.KeyType)
                .HasMaxLength(32)
                .IsRequired();

            builder.Property(uk => uk.Key)
                .IsRequired();


        }
    }
}