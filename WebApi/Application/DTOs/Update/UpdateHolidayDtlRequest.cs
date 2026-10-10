using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Application.DTOs.Update;
public sealed class UpdateHolidayDtlRequest
{
    public long SchoolId { get; init; }
    public long SessionId { get; init; }
    public DateOnly HolidayDate { get; init; }
    public string HolidayDescription { get; init; } = string.Empty;
}
