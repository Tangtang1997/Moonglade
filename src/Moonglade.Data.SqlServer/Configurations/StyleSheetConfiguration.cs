﻿using Microsoft.EntityFrameworkCore;
using Moonglade.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Moonglade.Data.SqlServer.Configurations;

internal class StyleSheetConfiguration : IEntityTypeConfiguration<StyleSheetEntity>
{
    public void Configure(EntityTypeBuilder<StyleSheetEntity> builder)
    {
        builder.Property(e => e.Id).ValueGeneratedNever();
        builder.Property(e => e.FriendlyName).HasMaxLength(32);
        builder.Property(e => e.Hash).HasMaxLength(64);
        builder.Property(e => e.LastModifiedTimeUtc).HasColumnType("datetime");
    }
}