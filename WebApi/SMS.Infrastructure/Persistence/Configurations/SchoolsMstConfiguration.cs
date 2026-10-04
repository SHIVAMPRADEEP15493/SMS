using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using SMS.Domain.Entities;

namespace SMS.Infrastructure.Persistence.Configurations;
public class SchoolsMstConfiguration : IEntityTypeConfiguration<SchoolMst>
{
    public void Configure(EntityTypeBuilder<SchoolMst> builder)
    {
        builder.ToTable("SMS_School_Mst");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.SchoolCode)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.SchoolName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.SchoolAddress)
            .HasMaxLength(500);

        builder.Property(x => x.City)
            .HasMaxLength(100);

        builder.Property(x => x.State)
            .HasMaxLength(100);

        builder.Property(x => x.PinCode)
            .HasMaxLength(20);

        builder.Property(x => x.PhoneNumber)
            .HasMaxLength(30);

        builder.Property(x => x.Email)
            .HasMaxLength(255);

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.HasIndex(x => x.SchoolCode)
            .IsUnique();

        builder.HasIndex(x => x.SchoolName);
    }
}
