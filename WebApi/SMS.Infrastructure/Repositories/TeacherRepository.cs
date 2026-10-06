using Microsoft.EntityFrameworkCore;
using Npgsql;
using SMS.Application.Interfaces;
using SMS.Domain.Entities;
using SMS.Infrastructure.Persistence;
using SMS.Infrastructure.Persistence.Exceptions;

namespace SMS.Infrastructure.Repositories;
public sealed class TeacherRepository : ITeacherRepository
{
    private readonly ApplicationDbContext _dbContext;
    public TeacherRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TeacherMst?> GetByIdAsync(long teacherId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.TeacherMst
            .FirstOrDefaultAsync(
                x => x.TeacherId == teacherId,
                cancellationToken);
    }
    public async Task<IReadOnlyList<TeacherMst>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.TeacherMst
            .AsNoTracking()
            .OrderBy(x => x.TeacherId)
            .ToListAsync(cancellationToken);
    }
    public async Task<bool> ExistsByTeacherCodeAsync(long schoolId, string teacherCode, long? excludeTeacherId = null, CancellationToken cancellationToken = default)
    {
        return await _dbContext.TeacherMst
            .AnyAsync(
                x =>
                    x.SchoolId == schoolId &&
                    x.TeacherCode == teacherCode &&
                    (!excludeTeacherId.HasValue ||
                     x.TeacherId != excludeTeacherId.Value),
                cancellationToken);
    }
    public async Task<bool> ExistsByEmployeeCodeAsync(long schoolId, string employeeCode, long? excludeTeacherId = null, CancellationToken cancellationToken = default)
    {
        return await _dbContext.TeacherMst
            .AnyAsync(
                x =>
                    x.SchoolId == schoolId &&
                    x.EmployeeCode == employeeCode &&
                    (!excludeTeacherId.HasValue ||
                     x.TeacherId != excludeTeacherId.Value),
                cancellationToken);
    }
    public async Task AddAsync(TeacherMst teacher, CancellationToken cancellationToken = default)
    {
        await _dbContext.TeacherMst.AddAsync(teacher, cancellationToken);
    }
    public async Task<bool> SchoolExistsAsync(long schoolId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.SchoolsMst.AsNoTracking()
            .AnyAsync(x => x.Id == schoolId, cancellationToken);
    }
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbContext.SaveChangesAsync(
                cancellationToken);
        }
        catch (PostgresException exception)
        {
            throw PostgresExceptionTranslator.Translate(
                exception);
        }
    }
}
