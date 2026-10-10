using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMS.Domain.Entities;

namespace SMS.Infrastructure.Persistence.Configurations;
public sealed class AttendanceTypeMstConfiguration : IEntityTypeConfiguration<AttendanceTypeMst>
{
    public void Configure(EntityTypeBuilder<AttendanceTypeMst> builder)
    {
        builder.ToTable("SMS_AttendanceType_Mst", table =>
        {
            table.HasCheckConstraint(
                "CK_SMS_Attendance_Type_Mst_Code_NotBlank",
                "\"AttendanceTypeCode\" = BTRIM(\"AttendanceTypeCode\") " +
                "AND LENGTH(\"AttendanceTypeCode\") > 0");

            table.HasCheckConstraint(
                "CK_SMS_Attendance_Type_Mst_Name_NotBlank",
                "\"AttendanceTypeName\" = BTRIM(\"AttendanceTypeName\") " +
                "AND LENGTH(\"AttendanceTypeName\") > 0");
        });

        // Primary key
        builder.HasKey(x => x.AttendanceTypeId)
            .HasName("PK_SMS_AttendanceType_Mst");

        builder.Property(x => x.AttendanceTypeId)
            .HasColumnName("PK_AttendanceTypeId")
            .UseIdentityByDefaultColumn();

        // Attendance type code
        builder.Property(x => x.AttendanceTypeCode)
            .HasColumnName("AttendanceTypeCode")
            .HasMaxLength(10)
            .IsRequired();

        // Attendance type name
        builder.Property(x => x.AttendanceTypeName)
            .HasColumnName("AttendanceTypeName")
            .HasMaxLength(50)
            .IsRequired();

        // Active/inactive flag
        builder.Property(x => x.IsActive)
            .HasColumnName("IsActive")
            .HasDefaultValue(true)
            .IsRequired();

        // Audit columns
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

        // Attendance type codes must be unique.
        builder.HasIndex(x => x.AttendanceTypeCode)
            .IsUnique()
            .HasDatabaseName(
                "UX_SMS_AttendanceType_Mst_AttendanceTypeCode");
    }
}
