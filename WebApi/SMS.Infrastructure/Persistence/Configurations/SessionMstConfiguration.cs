using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMS.Domain.Entities;

namespace SMS.Infrastructure.Persistence.Configurations;
public class SessionMstConfiguration : IEntityTypeConfiguration<SessionMst>
{
    public void Configure(
        EntityTypeBuilder<SessionMst> builder)
    {
        builder.ToTable(
            "SMS_Session_Mst",
            table =>
            {
                table.HasCheckConstraint(
                    "ck_sessions_date_range",
                    "start_date < end_date");

                table.HasCheckConstraint(
                    "ck_sessions_current_active",
                    "is_current = false OR is_active = true");
            });

        builder.HasKey(x => x.SessionId)
            .HasName("pk_sessions");

        builder.Property(x => x.SessionId)
            .HasColumnName("session_id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.SessionCode)
            .HasColumnName("session_code")
            .HasMaxLength(20)
            .IsRequired();

        builder.HasIndex(x => x.SessionCode)
            .IsUnique()
            .HasDatabaseName("ux_sessions_session_code");

        builder.Property(x => x.SessionName)
            .HasColumnName("session_name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.StartDate)
            .HasColumnName("start_date")
            .HasColumnType("date")
            .IsRequired();

        builder.Property(x => x.EndDate)
            .HasColumnName("end_date")
            .HasColumnType("date")
            .IsRequired();

        builder.Property(x => x.IsCurrent)
            .HasColumnName("is_current")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(x => x.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true)
            .IsRequired();

        builder.HasIndex(x => x.IsCurrent)
            .IsUnique()
            .HasFilter("is_current = true")
            .HasDatabaseName("ux_sessions_single_current");

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Property(x => x.CreatedBy)
            .HasColumnName("created_by");

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("timestamp with time zone");

        builder.Property(x => x.UpdatedBy)
            .HasColumnName("updated_by");

    }
}

