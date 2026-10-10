using SMS.Domain.Entities;
using SMS.Infrastructure.Persistence;
using SMS.Application.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace SMS.Infrastructure.Repositories;

public sealed class HolidayRepository : IHolidayRepository
{
    private readonly ApplicationDbContext _context;

    public HolidayRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<HolidayDtl>> GetAllAsync(
        long? schoolId,
        long? sessionId,
        CancellationToken cancellationToken = default)
    {
        IQueryable<HolidayDtl> query =
            _context.Set<HolidayDtl>().AsNoTracking();

        if (schoolId.HasValue)
        {
            query = query.Where(x => x.SchoolId == schoolId.Value);
        }

        if (sessionId.HasValue)
        {
            query = query.Where(x => x.SessionId == sessionId.Value);
        }

        return await query
            .OrderBy(x => x.HolidayDate)
            .ThenBy(x => x.HolidayId)
            .ToListAsync(cancellationToken);
    }

    public async Task<HolidayDtl?> GetByIdAsync(
        long holidayId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Set<HolidayDtl>()
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.HolidayId == holidayId,
                cancellationToken);
    }

    public async Task<bool> ExistsForSchoolSessionDateAsync(
        long schoolId,
        long sessionId,
        DateOnly holidayDate,
        long? excludeHolidayId = null,
        CancellationToken cancellationToken = default)
    {
        IQueryable<HolidayDtl> query = _context.Set<HolidayDtl>()
            .AsNoTracking()
            .Where(x =>
                x.SchoolId == schoolId &&
                x.SessionId == sessionId &&
                x.HolidayDate == holidayDate);

        if (excludeHolidayId.HasValue)
        {
            query = query.Where(
                x => x.HolidayId != excludeHolidayId.Value);
        }

        return await query.AnyAsync(cancellationToken);
    }

    public Task<bool> SchoolExistsAsync(
        long schoolId,
        CancellationToken cancellationToken = default)
    {
        return _context.Set<SchoolMst>()
            .AnyAsync(
                x => x.Id == schoolId,
                cancellationToken);
    }

    public Task<bool> SessionExistsAsync(
        long sessionId,
        CancellationToken cancellationToken = default)
    {
        return _context.Set<SessionMst>()
            .AnyAsync(
                x => x.SessionId == sessionId,
                cancellationToken);
    }

    public async Task AddAsync(
        HolidayDtl holiday,
        CancellationToken cancellationToken = default)
    {
        await _context.Set<HolidayDtl>()
            .AddAsync(holiday, cancellationToken);
    }

    public void Update(HolidayDtl holiday)
    {
        _context.Set<HolidayDtl>().Update(holiday);
    }

    public void Delete(HolidayDtl holiday)
    {
        _context.Set<HolidayDtl>().Remove(holiday);
    }

    public Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }

    public Task<SessionMst?> GetSessionByIdAsync(
    long sessionId,
    CancellationToken cancellationToken = default)
    {
        return _context.Set<SessionMst>()
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.SessionId == sessionId,
                cancellationToken);
    }
}
