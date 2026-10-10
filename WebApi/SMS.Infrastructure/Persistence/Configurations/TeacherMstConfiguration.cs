using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Infrastructure.Persistence.Configurations;
public sealed class TeacherMstConfiguration : IEntityTypeConfiguration<TeacherMst>
{
    public void Configure(
        EntityTypeBuilder<TeacherMst> builder)
    {
        builder.ToTable("SMS_Teacher_Mst");

        // Primary Key
        builder.HasKey(x => x.TeacherId)
            .HasName("pk_teacher");

        builder.Property(x => x.TeacherId)
            .HasColumnName("PK_TeacherId")
            .ValueGeneratedOnAdd();

        // School
        builder.Property(x => x.SchoolId)
            .HasColumnName("FK_SchoolId")
            .IsRequired();

        // Teacher Code
        builder.Property(x => x.TeacherCode)
            .HasColumnName("TeacherCode")
            .HasMaxLength(20)
            .IsRequired();

        // Employee Code
        builder.Property(x => x.EmployeeCode)
            .HasColumnName("EmployeeCode")
            .HasMaxLength(30)
            .IsRequired();

        // Name
        builder.Property(x => x.FirstName)
            .HasColumnName("FirstName")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.MiddleName)
            .HasColumnName("MiddleName")
            .HasMaxLength(50);

        builder.Property(x => x.LastName)
            .HasColumnName("LastName")
            .HasMaxLength(50)
            .IsRequired();

        // Gender
        builder.Property(x => x.Gender)
            .HasColumnName("Gender")
            .HasConversion<short>()
            .IsRequired();

        // Date of Birth
        builder.Property(x => x.DateOfBirth)
            .HasColumnName("DateOfBirth")
            .HasColumnType("date");

        // Contact
        builder.Property(x => x.Email)
            .HasColumnName("Email")
            .HasMaxLength(150);

        builder.Property(x => x.MobileNumber)
            .HasColumnName("MobileNumber")
            .HasMaxLength(20);

        // Employment
        builder.Property(x => x.DateOfJoining)
            .HasColumnName("DateOfJoining")
            .HasColumnType("date")
            .IsRequired();

        builder.Property(x => x.Qualification)
            .HasColumnName("Qualification")
            .HasMaxLength(200);

        builder.Property(x => x.Designation)
            .HasColumnName("Designation")
            .HasMaxLength(100);

        // Status
        builder.Property(x => x.IsActive)
            .HasColumnName("IsActive")
            .HasDefaultValue(true)
            .IsRequired();

        // Audit
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

        // Foreign Key
        builder.HasOne<SchoolMst>()
            .WithMany()
            .HasForeignKey(x => x.SchoolId)
            .HasConstraintName("fk_teacher_school")
            .OnDelete(DeleteBehavior.Restrict);

        // Unique Teacher Code within School
        builder.HasIndex(x => new
            {
                x.SchoolId,
                x.TeacherCode
            })
            .IsUnique()
            .HasDatabaseName(
                "ux_teacher_school_teacher_code");

        // Unique Employee Code within School
        builder.HasIndex(x => new
            {
                x.SchoolId,
                x.EmployeeCode
            })
            .IsUnique()
            .HasDatabaseName(
                "ux_teacher_school_employee_code");

        // Useful lookup index
        builder.HasIndex(x => x.SchoolId)
            .HasDatabaseName("ix_teacher_school_id");
    }
}
