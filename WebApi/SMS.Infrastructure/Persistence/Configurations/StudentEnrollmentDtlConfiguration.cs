using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMS.Domain.Entities;

namespace SMS.Infrastructure.Persistence.Configurations;

public sealed class StudentEnrollmentDtlConfiguration : IEntityTypeConfiguration<StudentEnrollmentDtl>
{
    public void Configure(
        EntityTypeBuilder<StudentEnrollmentDtl> builder)
    {
        builder.ToTable(
            "SMS_StudentEnrollment_Dtl",
            table =>
            {
                table.HasCheckConstraint(
                    "CK_SMS_Student_Enrollment_Dtl_Status",
                    "\"Status\" IN ('Active', 'Completed', 'Transferred', 'Withdrawn')");

                table.HasCheckConstraint(
                    "CK_SMS_Student_Enrollment_Dtl_AdmissionDate",
                    "\"AdmissionDate\" >= DATE '1900-01-01'");
            });

        // Primary Key
        builder.HasKey(x => x.StudentEnrollmentId)
            .HasName("PK_StudentEnrollmentId");

        builder.Property(x => x.StudentEnrollmentId)
            .HasColumnName("PK_StudentEnrollmentId")
            .UseIdentityByDefaultColumn();

        // Student / School / Session / Class / Section
        builder.Property(x => x.StudentId)
            .HasColumnName("FK_StudentId")
            .IsRequired();

        builder.Property(x => x.SchoolId)
            .HasColumnName("FK_SchoolId")
            .IsRequired();

        builder.Property(x => x.SessionId)
            .HasColumnName("FK_SessionId")
            .IsRequired();

        builder.Property(x => x.ClassId)
            .HasColumnName("FK_ClassId")
            .IsRequired();

        builder.Property(x => x.SectionId)
            .HasColumnName("FK_SectionId")
            .IsRequired();

        // Admission Information
        builder.Property(x => x.AdmissionDate)
            .HasColumnName("AdmissionDate")
            .HasColumnType("date")
            .IsRequired();

        builder.Property(x => x.RollNumber)
            .HasColumnName("RollNumber")
            .HasMaxLength(20)
            .IsRequired();

        // Enrollment Lifecycle
        builder.Property(x => x.Status)
            .HasColumnName("Status")
            .HasMaxLength(20)
            .HasDefaultValue("Active")
            .IsRequired();

        // Audit
        builder.Property(x => x.CreatedAt)
            .HasColumnName("CreatedAt")
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.Property(x => x.CreatedBy)
            .HasColumnName("CreatedBy");

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("UpdatedAt")
            .HasColumnType("timestamp with time zone");

        builder.Property(x => x.UpdatedBy)
            .HasColumnName("UpdatedBy");

        // A student may have only one enrollment per academic session.
        builder.HasIndex(x => new
        {
            x.StudentId,
            x.SessionId
        })
            .IsUnique()
            .HasDatabaseName(
                "UX_SMS_StudentEnrollment_Dtl_StudentId_SessionId");
    }
}
