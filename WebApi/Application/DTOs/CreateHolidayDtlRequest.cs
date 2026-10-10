using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Application.DTOs;
public sealed class CreateHolidayDtlRequest
{
    public long SchoolId { get; init; }
    public long SessionId { get; init; }
    public DateOnly HolidayDate { get; init; }
    public string HolidayDescription { get; init; } = null!;
}
