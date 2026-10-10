using SMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Application.Interfaces;

public interface IHolidayRepository
{
    Task<IReadOnlyList<HolidayDtl>> GetAllAsync(
        long? schoolId,
        long? sessionId,
        CancellationToken cancellationToken = default);

    Task<HolidayDtl?> GetByIdAsync(
        long holidayId,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsForSchoolSessionDateAsync(
        long schoolId,
        long sessionId,
        DateOnly holidayDate,
        long? excludeHolidayId = null,
        CancellationToken cancellationToken = default);

    Task<bool> SchoolExistsAsync(
        long schoolId,
        CancellationToken cancellationToken = default);

    Task<bool> SessionExistsAsync(
        long sessionId,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        HolidayDtl holiday,
        CancellationToken cancellationToken = default);

    void Update(HolidayDtl holiday);

    void Delete(HolidayDtl holiday);

    Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default);

    Task<SessionMst?> GetSessionByIdAsync(
    long sessionId,
    CancellationToken cancellationToken = default);
}
