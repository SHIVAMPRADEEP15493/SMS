using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Application.DTOs;
public sealed class StudentMstResponse
{
    public long StudentId { get; init; }

    // Identity
    public string StudentCode { get; init; } = null!;
    public string AdmissionNumber { get; init; } = null!;

    // Admission Information
    public DateOnly AdmissionDate { get; init; }
    public long AdmittedSessionId { get; init; }

    // Personal Information
    public string FirstName { get; init; } = null!;
    public string? MiddleName { get; init; }
    public string? LastName { get; init; }
    public DateOnly DateOfBirth { get; init; }
    public string Gender { get; init; } = null!;

    // Contact Information
    public string? MobileNumber { get; init; }
    public string? Email { get; init; }
    public string? Address { get; init; }

    // Parent / Guardian Information
    public string? FatherName { get; init; }
    public string? MotherName { get; init; }
    public string? GuardianName { get; init; }
    public string? GuardianMobileNumber { get; init; }

    // Lifecycle
    public string CurrentStatus { get; init; } = null!;
    public bool IsActive { get; init; }

    // Audit
    public DateTime CreatedAt { get; init; }
    public long? CreatedBy { get; init; }
    public DateTime? UpdatedAt { get; init; }
    public long? UpdatedBy { get; init; }
    //public long Version { get; set; }
}
