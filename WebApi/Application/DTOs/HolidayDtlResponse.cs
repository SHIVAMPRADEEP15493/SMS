using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Application.DTOs;
public sealed class HolidayDtlResponse
{
    public long HolidayId { get; init; }
    public long SchoolId { get; init; }
    public long SessionId { get; init; }
    public DateOnly HolidayDate { get; init; }
    public string HolidayDescription { get; init; } = string.Empty;
    public DateTimeOffset CreatedAt { get; init; }
    public long? CreatedBy { get; init; }
    public DateTimeOffset? UpdatedAt { get; init; }
    public long? UpdatedBy { get; init; }
}
