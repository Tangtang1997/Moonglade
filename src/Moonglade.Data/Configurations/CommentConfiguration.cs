﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moonglade.Data.Entities;

namespace Moonglade.Data.Configurations
{
    internal class CommentConfiguration : IEntityTypeConfiguration<CommentEntity>
    {
        public void Configure(EntityTypeBuilder<CommentEntity> builder)
        {
            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(e => e.CommentContent).IsRequired();
            builder.Property(e => e.CreateOnUtc).HasColumnType("datetime");
            builder.Property(e => e.Email).HasMaxLength(128);
            builder.Property(e => e.IPAddress).HasMaxLength(64);
            builder.Property(e => e.Username).HasMaxLength(64);
            builder.HasOne(d => d.Post)
                   .WithMany(p => p.Comment)
                   .HasForeignKey(d => d.PostId)
                   .HasConstraintName("FK_Comment_Post");
        }
    }
}
