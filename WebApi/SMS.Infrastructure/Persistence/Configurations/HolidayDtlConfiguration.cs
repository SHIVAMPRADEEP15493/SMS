using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Infrastructure.Persistence.Configurations;
public class HolidayDtlConfiguration : IEntityTypeConfiguration<HolidayDtl>
{
    public void Configure(EntityTypeBuilder<HolidayDtl> builder)
    {
        builder.ToTable("SMS_Holiday_Dtl");

        // Primary Key
        builder.HasKey(x => x.HolidayId)
            .HasName("PK_HolidayId");

        builder.Property(x => x.HolidayId)
            .HasColumnName("PK_HolidayId")
            .UseIdentityByDefaultColumn();

        // School Foreign Key
        builder.Property(x => x.SchoolId)
            .HasColumnName("FK_SchoolId")
            .IsRequired();

        // Session Foreign Key
        builder.Property(x => x.SessionId)
            .HasColumnName("FK_SessionId")
            .IsRequired();

        // Holiday Date
        builder.Property(x => x.HolidayDate)
            .HasColumnName("HolidayDate")
            .HasColumnType("date")
            .IsRequired();

        // Holiday Description
        builder.Property(x => x.HolidayDescription)
            .HasColumnName("HolidayDescription")
            .HasMaxLength(200)
            .IsRequired();

        // Audit Columns
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

        // Foreign Key: School
        builder.HasOne<SchoolMst>()
            .WithMany()
            .HasForeignKey(x => x.SchoolId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_HolidayDtl_SchoolMst");

        // Foreign Key: Session
        builder.HasOne<SessionMst>()
            .WithMany()
            .HasForeignKey(x => x.SessionId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_HolidayDtl_SessionMst");

        // A school cannot have two holiday records
        // for the same session and date.
        builder.HasIndex(x => new
        {
            x.SchoolId,
            x.SessionId,
            x.HolidayDate
        })
            .IsUnique()
            .HasDatabaseName("UX_HolidayDtl_School_Session_Date");
    }
}
