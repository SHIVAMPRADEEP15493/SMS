using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Infrastructure.Persistence.Configurations;
public sealed class ClassMstConfiguration : IEntityTypeConfiguration<ClassMst>
{
    public void Configure(
         EntityTypeBuilder<ClassMst> builder)
    {
        builder.ToTable(
            "SMS_Class_Mst",
            table =>
            {
                table.HasCheckConstraint(
                    "ck_class_display_order",
                    "\"DisplayOrder\" > 0");
            });

        // Primary Key
        builder.HasKey(x => x.ClassId)
            .HasName("PK_Class");

        builder.Property(x => x.ClassId)
            .HasColumnName("PK_ClassId")
            .ValueGeneratedOnAdd();

        // Class Code
        builder.Property(x => x.ClassCode)
            .HasColumnName("ClassCode")
            .HasMaxLength(20)
            .IsRequired();

        builder.HasIndex(x => x.ClassCode)
            .IsUnique()
            .HasDatabaseName("UX_Class_Code");

        // Class Name
        builder.Property(x => x.ClassName)
            .HasColumnName("ClassName")
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(x => x.ClassName)
            .IsUnique()
            .HasDatabaseName("UX_Class_Name");

        // Display Order
        builder.Property(x => x.DisplayOrder)
            .HasColumnName("DisplayOrder")
            .IsRequired();

        builder.HasIndex(x => x.DisplayOrder)
            .IsUnique()
            .HasDatabaseName("UX_Class_DisplayOrder");

        // Active Flag
        builder.Property(x => x.IsActive)
            .HasColumnName("IsActive")
            .HasDefaultValue(true)
            .IsRequired();

        // Created Audit
        builder.Property(x => x.CreatedAt)
            .HasColumnName("CreatedAt")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Property(x => x.CreatedBy)
            .HasColumnName("CreatedBy");

        // Updated Audit
        builder.Property(x => x.UpdatedAt)
            .HasColumnName("UpdatedAt")
            .HasColumnType("timestamp with time zone");

        builder.Property(x => x.UpdatedBy)
            .HasColumnName("UpdatedBy");
    }
}
