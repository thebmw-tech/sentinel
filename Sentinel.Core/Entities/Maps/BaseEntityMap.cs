using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sentinel.Core.Entities.Maps
{
    public abstract class BaseEntityMap<T> : IEntityTypeConfiguration<T> where T : BaseEntity<T>
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            
        }

        public static Type GetEntityType()
        {
            return typeof(T);
        }
    }
}
