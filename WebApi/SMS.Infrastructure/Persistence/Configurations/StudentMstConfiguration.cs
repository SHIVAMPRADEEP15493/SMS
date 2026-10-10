using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMS.Domain.Entities;

namespace SMS.Infrastructure.Persistence.Configurations;
public class StudentMstConfiguration : IEntityTypeConfiguration<StudentMst>
{
    public void Configure(EntityTypeBuilder<StudentMst> builder)
    {
        builder.ToTable("SMS_Student_Mst");

        // Primary Key
        builder.HasKey(x => x.StudentId)
            .HasName("PK_StudentId");

        builder.Property(x => x.StudentId)
            .HasColumnName("PK_StudentId")
            .ValueGeneratedOnAdd();

        // Identity
        builder.Property(x => x.StudentCode)
            .HasColumnName("StudentCode")
            .HasMaxLength(30)
            .IsRequired();

        builder.HasIndex(x => x.StudentCode)
            .IsUnique()
            .HasDatabaseName("UX_StudentCode");

        builder.Property(x => x.AdmissionNumber)
            .HasColumnName("AdmissionNumber")
            .HasMaxLength(30)
            .IsRequired();

        // Admission Information
        builder.Property(x => x.AdmissionDate)
            .HasColumnName("AdmissionDate")
            .HasColumnType("date")
            .IsRequired();

        builder.Property(x => x.AdmittedSessionId)
            .HasColumnName("AdmittedSessionId")
            .IsRequired();

        // Personal Information
        builder.Property(x => x.FirstName)
            .HasColumnName("FirstName")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.MiddleName)
            .HasColumnName("MiddleName")
            .HasMaxLength(100);

        builder.Property(x => x.LastName)
            .HasColumnName("LastName")
            .HasMaxLength(100);

        builder.Property(x => x.DateOfBirth)
            .HasColumnName("DateOfBirth")
            .HasColumnType("date")
            .IsRequired();

        builder.Property(x => x.Gender)
            .HasColumnName("Gender")
            .HasMaxLength(20)
            .IsRequired();

        // Contact Information
        builder.Property(x => x.MobileNumber)
            .HasColumnName("MobileNumber")
            .HasMaxLength(20);

        builder.Property(x => x.Email)
            .HasColumnName("Email")
            .HasMaxLength(150);

        builder.Property(x => x.Address)
            .HasColumnName("Address")
            .HasMaxLength(500);

        // Parent / Guardian Information
        builder.Property(x => x.FatherName)
            .HasColumnName("FatherName")
            .HasMaxLength(200);

        builder.Property(x => x.MotherName)
            .HasColumnName("MotherName")
            .HasMaxLength(200);

        builder.Property(x => x.GuardianName)
            .HasColumnName("GuardianName")
            .HasMaxLength(200);

        builder.Property(x => x.GuardianMobileNumber)
            .HasColumnName("GuardianMobileNumber")
            .HasMaxLength(20);

        // Lifecycle
        builder.Property(x => x.CurrentStatus)
            .HasColumnName("CurrentStatus")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

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

        //builder.Property(x => x.Version)
        //       .IsRequired()
        //       .IsConcurrencyToken()
        //       .HasDefaultValue(1L);
    }
}


