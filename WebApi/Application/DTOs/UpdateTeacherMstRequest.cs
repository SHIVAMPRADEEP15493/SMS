using SMS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Application.DTOs;
public sealed class UpdateTeacherMstRequest
{
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
}
