﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Sentinel.Core.Entities.Maps
{
    public class UserMap : BaseEntityMap<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Username)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(u => u.Password)
                .HasMaxLength(100)
                .IsRequired();


            builder.HasIndex(u => u.Username)
                .IsUnique();

            builder.HasMany(u => u.Keys)
                .WithOne(uk => uk.User);
        }
    }
}