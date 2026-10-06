using SMS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Application.DTOs;
public sealed class TeacherMstResponse
{
    public long TeacherId { get; init; }
    public long SchoolId { get; init; }
    public string TeacherCode { get; init; } = null!;
    public string EmployeeCode { get; init; } = null!;
    public string FirstName { get; init; } = null!;
    public string? MiddleName { get; init; }
    public string LastName { get; init; } = null!;
    public Gender Gender { get; init; }
    public DateOnly? DateOfBirth { get; init; }
    public string? Email { get; init; }
    public string? MobileNumber { get; init; }
    public DateOnly DateOfJoining { get; init; }
    public string? Qualification { get; init; }
    public string? Designation { get; init; }
    public bool IsActive { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public long? CreatedBy { get; init; }
    public DateTimeOffset? UpdatedAt { get; init; }
    public long? UpdatedBy { get; init; }
}
