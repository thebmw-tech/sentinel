﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sentinel.Core.Entities.Maps
{
    public class RevisionMap : BaseEntityMap<Revision>
    {
        public override void Configure(EntityTypeBuilder<Revision> builder)
        {
            base.Configure(builder);

            builder.HasKey(r => r.Id);

            builder.Property(r => r.CreatedDate)
                .HasDefaultValueSql("DATETIME('now')");

            builder.HasIndex(r => r.CommitDate);
            builder.HasIndex(r => r.ConfirmDate);
        }
    }
}