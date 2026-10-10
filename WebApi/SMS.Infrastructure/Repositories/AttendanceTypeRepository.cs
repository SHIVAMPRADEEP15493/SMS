using Microsoft.EntityFrameworkCore;
using SMS.Domain.Entities;
using SMS.Infrastructure.Persistence;
using SMS.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Infrastructure.Repositories;
public sealed class AttendanceTypeRepository : IAttendanceTypeRepository
{
    private readonly ApplicationDbContext _context;

    public AttendanceTypeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        AttendanceTypeMst attendanceType,
        CancellationToken cancellationToken = default)
    {
        await _context.Set<AttendanceTypeMst>()
            .AddAsync(attendanceType, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<AttendanceTypeMst?> GetByIdAsync(
        long attendanceTypeId,
        CancellationToken cancellationToken = default)
    {
        // Tracking is enabled because the service may update this entity.
        return await _context.Set<AttendanceTypeMst>()
            .FirstOrDefaultAsync(
                x => x.AttendanceTypeId == attendanceTypeId,
                cancellationToken);
    }

    public async Task<AttendanceTypeMst?> GetByCodeAsync(
        string attendanceTypeCode,
        CancellationToken cancellationToken = default)
    {
        var normalizedCode = NormalizeCode(attendanceTypeCode);

        return await _context.Set<AttendanceTypeMst>()
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.AttendanceTypeCode == normalizedCode,
                cancellationToken);
    }

    public async Task<IReadOnlyList<AttendanceTypeMst>> GetAllAsync(
        bool activeOnly = false,
        CancellationToken cancellationToken = default)
    {
        IQueryable<AttendanceTypeMst> query =
            _context.Set<AttendanceTypeMst>().AsNoTracking();

        if (activeOnly)
        {
            query = query.Where(x => x.IsActive);
        }

        return await query
            .OrderBy(x => x.AttendanceTypeCode)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByCodeAsync(
        string attendanceTypeCode,
        long? excludeAttendanceTypeId = null,
        CancellationToken cancellationToken = default)
    {
        var normalizedCode = NormalizeCode(attendanceTypeCode);

        IQueryable<AttendanceTypeMst> query =
            _context.Set<AttendanceTypeMst>()
                .AsNoTracking()
                .Where(x => x.AttendanceTypeCode == normalizedCode);

        if (excludeAttendanceTypeId.HasValue)
        {
            query = query.Where(
                x => x.AttendanceTypeId != excludeAttendanceTypeId.Value);
        }

        return await query.AnyAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        AttendanceTypeMst attendanceType,
        CancellationToken cancellationToken = default)
    {
        _context.Entry(attendanceType).State = EntityState.Modified;

        await _context.SaveChangesAsync(cancellationToken);
    }

    private static string NormalizeCode(string code)
    {
        return code.Trim().ToUpperInvariant();
    }
}
