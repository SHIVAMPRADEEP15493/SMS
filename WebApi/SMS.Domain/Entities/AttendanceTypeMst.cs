using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Domain.Entities;

public sealed class AttendanceTypeMst
{
    public long AttendanceTypeId { get; private set; }
    public string AttendanceTypeCode { get; private set; } = string.Empty;
    public string AttendanceTypeName { get; private set; } = string.Empty;
    public bool IsActive { get; private set; } = true;
    public DateTime CreatedAt { get; private set; }
    public long? CreatedBy { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public long? UpdatedBy { get; private set; }

    // Required by Entity Framework Core.
    private AttendanceTypeMst()
    {
    }

    public AttendanceTypeMst(
        string attendanceTypeCode,
        string attendanceTypeName,
        long? createdBy = null)
    {
        AttendanceTypeCode = NormalizeCode(attendanceTypeCode);
        AttendanceTypeName = NormalizeName(attendanceTypeName);
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        CreatedBy = createdBy;
    }

    public void Update(
        string attendanceTypeCode,
        string attendanceTypeName,
        long? updatedBy = null)
    {
        AttendanceTypeCode = NormalizeCode(attendanceTypeCode);
        AttendanceTypeName = NormalizeName(attendanceTypeName);
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
    }

    public void Activate(long? updatedBy = null)
    {
        if (IsActive)
        {
            return;
        }

        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
    }

    public void Deactivate(long? updatedBy = null)
    {
        if (!IsActive)
        {
            return;
        }

        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
    }

    private static string NormalizeCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException(
                "Attendance type code is required.",
                nameof(code));
        }

        var normalizedCode = code.Trim().ToUpperInvariant();

        if (normalizedCode.Length > 10)
        {
            throw new ArgumentException(
                "Attendance type code cannot exceed 10 characters.",
                nameof(code));
        }

        return normalizedCode;
    }

    private static string NormalizeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(
                "Attendance type name is required.",
                nameof(name));
        }

        var normalizedName = name.Trim();

        if (normalizedName.Length > 50)
        {
            throw new ArgumentException(
                "Attendance type name cannot exceed 50 characters.",
                nameof(name));
        }

        return normalizedName;
    }
}
