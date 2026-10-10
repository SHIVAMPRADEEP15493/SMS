using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Domain.Entities;
public class HolidayDtl
{
    public long HolidayId { get; set; }

    public long SchoolId { get; set; }

    public long SessionId { get; set; }

    public DateOnly HolidayDate { get; set; }

    public string HolidayDescription { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; }

    public long? CreatedBy { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }

    public long? UpdatedBy { get; set; }
}
