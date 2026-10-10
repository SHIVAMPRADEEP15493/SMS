using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Infrastructure.Persistence.Configurations;
public sealed class SectionMstConfiguration : IEntityTypeConfiguration<SectionMst>
{
    public void Configure(EntityTypeBuilder<SectionMst> builder)
    {
        builder.ToTable(
            "SMS_Section_Mst",
            table =>
            {
                table.HasCheckConstraint(
                    "CK_Section_DisplayOrder",
                    "\"DisplayOrder\" > 0");
            });

        // Primary Key
        builder.HasKey(x => x.SectionId)
            .HasName("PK_SectionId");

        builder.Property(x => x.SectionId)
            .HasColumnName("PK_SectionId")
            .ValueGeneratedOnAdd();

        // Section Code
        builder.Property(x => x.SectionCode)
            .HasColumnName("SectionCode")
            .HasMaxLength(20)
            .IsRequired();

        builder.HasIndex(x => x.SectionCode)
            .IsUnique()
            .HasDatabaseName("UX_Section_Code");

        // Section Name
        builder.Property(x => x.SectionName)
            .HasColumnName("SectionName")
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(x => x.SectionName)
            .IsUnique()
            .HasDatabaseName("UX_Section_Name");

        // Display Order
        builder.Property(x => x.DisplayOrder)
            .HasColumnName("DisplayOrder")
            .IsRequired();

        builder.HasIndex(x => x.DisplayOrder)
            .IsUnique()
            .HasDatabaseName("UX_Section_DisplayOrder");

        // Active Status
        builder.Property(x => x.IsActive)
            .HasColumnName("IsActive")
            .HasDefaultValue(true)
            .IsRequired();

        // Audit Fields
        builder.Property(x => x.CreatedAt)
            .HasColumnName("CreatedAt")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Property(x => x.CreatedBy)
            .HasColumnName("CreatedBy");

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("UpdatedAt")
            .HasColumnType("timestamp with time zone");

        builder.Property(x => x.UpdatedBy)
            .HasColumnName("UpdatedBy");
    }
}
