﻿namespace FWTL.Database.Configuration
{
    using FWTL.Core.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class TelegramSessionConfiguration : IEntityTypeConfiguration<TelegramSession>
    {
        public void Configure(EntityTypeBuilder<TelegramSession> builder)
        {
            builder.HasKey(x => x.UserId);
            builder.Property(x => x.UserId).IsRequired().ValueGeneratedNever();
            builder.Property(b => b.Session).IsRequired();
        }
    }
}
