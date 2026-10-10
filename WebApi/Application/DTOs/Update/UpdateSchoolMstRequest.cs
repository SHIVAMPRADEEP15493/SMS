using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Application.DTOs.Update;
public sealed class UpdateSchoolMstRequest
{
    public string SchoolName { get; init; } = null!;
    public string? SchoolAddress { get; init; }
    public string? City { get; init; }
    public string? State { get; init; }
    public string? Country { get; init; }
    public string? PinCode { get; init; }
    public string? PhoneNumber { get; init; }
    public string? Email { get; init; }
}
