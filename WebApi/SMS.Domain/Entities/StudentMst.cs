using System;
using System.Collections.Generic;
using System.Text;
using SMS.Domain.Enums;

namespace SMS.Domain.Entities;
public sealed class StudentMst
{
    public long StudentId { get; private set; }

    // Identity
    public string StudentCode { get; private set; } = null!;
    public string AdmissionNumber { get; private set; } = null!;

    // Admission Information
    public DateOnly AdmissionDate { get; private set; }
    public long AdmittedSessionId { get; private set; }

    // Personal Information
    public string FirstName { get; private set; } = null!;
    public string? MiddleName { get; private set; }
    public string? LastName { get; private set; }
    public DateOnly DateOfBirth { get; private set; }
    public string Gender { get; private set; } = null!;

    // Contact Information
    public string? MobileNumber { get; private set; }
    public string? Email { get; private set; }
    public string? Address { get; private set; }

    // Parent / Guardian Information
    public string? FatherName { get; private set; }
    public string? MotherName { get; private set; }
    public string? GuardianName { get; private set; }
    public string? GuardianMobileNumber { get; private set; }

    // Audit / Lifecycle
    public StudentStatus CurrentStatus { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public long? CreatedBy { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public long? UpdatedBy { get; private set; }
    //public long Version { get; private set; } = 1;

    // EF Core constructor
    private StudentMst()
    {
    }
    public StudentMst(
        string studentCode,
        string admissionNumber,
        DateOnly admissionDate,
        long admittedSessionId,
        string firstName,
        string? middleName,
        string? lastName,
        DateOnly dateOfBirth,
        string gender,
        string? mobileNumber,
        string? guardianMobileNumber,
        string? email,
        string? address,
        string? fatherName,
        string? motherName,
        string? guardianName,
        long? createdBy = null)
    {
        StudentCode = studentCode;
        AdmissionNumber = admissionNumber;

        AdmissionDate = admissionDate;
        AdmittedSessionId = admittedSessionId;

        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        Gender = gender;

        MobileNumber = mobileNumber;
        Email = email;
        Address = address;

        FatherName = fatherName;
        MotherName = motherName;
        GuardianName = guardianName;
        GuardianMobileNumber = guardianMobileNumber;

        CurrentStatus = StudentStatus.Active;
        IsActive = true;

        CreatedAt = DateTime.UtcNow;
        CreatedBy = createdBy;
    }
    public void Update(
        string firstName,
        string? middleName,
        string? lastName,
        DateOnly dateOfBirth,
        string gender,
        string? mobileNumber,
        string? email,
        string? address,
        string? fatherName,
        string? motherName,
        string? guardianName,
        string? guardianMobileNumber,
        long? updatedBy = null
        //,long version
        )
    {
        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;

        DateOfBirth = dateOfBirth;
        Gender = gender;

        MobileNumber = mobileNumber;
        Email = email;
        Address = address;

        FatherName = fatherName;
        MotherName = motherName;
        GuardianName = guardianName;
        GuardianMobileNumber = guardianMobileNumber;

        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
        //Version = checked(version + 1);
    }
    public void UpdateStatus(StudentStatus status, long? updatedBy = null)
    {
        CurrentStatus = status;
        IsActive = status == StudentStatus.Active;

        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
        //Version = checked(Version + 1);
    }
    public void Deactivate(long? updatedBy = null)
    {
        IsActive = false;

        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
    }
    public void Activate(long? updatedBy = null)
    {
        IsActive = true;

        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
    }
}
