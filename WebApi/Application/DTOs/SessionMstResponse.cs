using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Application.DTOs;
public sealed class SessionMstResponse
{
    public long SessionId { get; init; }
    public string SessionCode { get; init; } = string.Empty;
    public string SessionName { get; init; } = string.Empty;
    public DateOnly StartDate { get; init; }
    public DateOnly EndDate { get; init; }
    public bool IsCurrent { get; init; }
    public bool IsActive { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public long? CreatedBy { get; init; }
    public DateTimeOffset? UpdatedAt { get; init; }
    public long? UpdatedBy { get; init; }
}
