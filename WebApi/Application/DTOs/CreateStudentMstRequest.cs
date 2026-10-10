using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Application.DTOs;
public sealed class CreateStudentMstRequest
{
    // Identity
    public string StudentCode { get; set; } = null!;
    public string AdmissionNumber { get; set; } = null!;

    // Admission Information
    public DateOnly AdmissionDate { get; set; }
    public long AdmittedSessionId { get; set; }

    // Personal Information
    public string FirstName { get; set; } = null!;
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public string Gender { get; set; } = null!;

    // Contact Information
    public string? MobileNumber { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }

    // Parent / Guardian Information
    public string? FatherName { get; set; }
    public string? MotherName { get; set; }
    public string? GuardianName { get; set; }
    public string? GuardianMobileNumber { get; set; }

    // Audit
    public long? CreatedBy { get; set; }
}
