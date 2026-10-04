using System;
using System.Collections.Generic;
using System.Text;
using SMS.Application.DTOs;

namespace IntegrationTests.TestData;
public static class SessionTestData
{
    public static CreateSessionMstRequest CreateRequest(
        string code = "2026-27",
        string name = "Academic Session 2026-27",
        DateOnly? startDate = null,
        DateOnly? endDate = null,
        bool isCurrent = false)
    {
        return new CreateSessionMstRequest
        {
            SessionCode = code,
            SessionName = name,
            StartDate = startDate ?? new DateOnly(2026, 4, 1),
            EndDate = endDate ?? new DateOnly(2027, 3, 31),
            IsCurrent = isCurrent
        };
    }

    public static UpdateSessionMstRequest CreateUpdateRequest(string? name = null, DateOnly? startDate = null, DateOnly? endDate = null)
    {
        return new UpdateSessionMstRequest
        {
            SessionName = name ?? "Updated Academic Session",
            StartDate = startDate ?? new DateOnly(2026, 4, 1),
            EndDate = endDate ?? new DateOnly(2027, 3, 31)
        };
    }
}
