using SMS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Domain.Entities;
public sealed class TeacherMst
{
    private TeacherMst()
    {
    }
    public long TeacherId { get; private set; }
    public long SchoolId { get; private set; }
    public string TeacherCode { get; private set; } = null!;
    public string EmployeeCode { get; private set; } = null!;
    public string FirstName { get; private set; } = null!;
    public string? MiddleName { get; private set; }
    public string LastName { get; private set; } = null!;
    public Gender Gender { get; private set; }
    public DateOnly? DateOfBirth { get; private set; }
    public string? Email { get; private set; }
    public string? MobileNumber { get; private set; }
    public DateOnly DateOfJoining { get; private set; }
    public string? Qualification { get; private set; }
    public string? Designation { get; private set; }
    public bool IsActive { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public long? CreatedBy { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }
    public long? UpdatedBy { get; private set; }
    public static TeacherMst Create(
        long schoolId,
        string teacherCode,
        string employeeCode,
        string firstName,
        string? middleName,
        string lastName,
        Gender gender,
        DateOnly? dateOfBirth,
        string? email,
        string? mobileNumber,
        DateOnly dateOfJoining,
        string? qualification,
        string? designation,
        long? createdBy,
        DateTimeOffset createdAt)
    {
        return new TeacherMst
        {
            SchoolId = schoolId,
            TeacherCode = teacherCode,
            EmployeeCode = employeeCode,
            FirstName = firstName,
            MiddleName = middleName,
            LastName = lastName,
            Gender = gender,
            DateOfBirth = dateOfBirth,
            Email = email,
            MobileNumber = mobileNumber,
            DateOfJoining = dateOfJoining,
            Qualification = qualification,
            Designation = designation,
            IsActive = true,
            CreatedBy = createdBy,
            CreatedAt = createdAt
        };
    }
    public void UpdateDetails(
        string firstName,
        string? middleName,
        string lastName,
        Gender gender,
        DateOnly? dateOfBirth,
        string? email,
        string? mobileNumber,
        DateOnly dateOfJoining,
        string? qualification,
        string? designation,
        long? updatedBy,
        DateTimeOffset updatedAt)
    {
        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;
        Gender = gender;
        DateOfBirth = dateOfBirth;
        Email = email;
        MobileNumber = mobileNumber;
        DateOfJoining = dateOfJoining;
        Qualification = qualification;
        Designation = designation;

        SetUpdatedAudit(updatedBy, updatedAt);
    }
    public void Activate(
        long? updatedBy,
        DateTimeOffset updatedAt)
    {
        if (IsActive)
        {
            return;
        }

        IsActive = true;

        SetUpdatedAudit(updatedBy, updatedAt);
    }

    public void Deactivate(
        long? updatedBy,
        DateTimeOffset updatedAt)
    {
        if (!IsActive)
        {
            return;
        }

        IsActive = false;

        SetUpdatedAudit(updatedBy, updatedAt);
    }

    private void SetUpdatedAudit(
        long? updatedBy,
        DateTimeOffset updatedAt)
    {
        UpdatedBy = updatedBy;
        UpdatedAt = updatedAt;
    }
}
