using SMS.Application.Interfaces;
using SMS.Domain.Entities;
using SMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace SMS.Infrastructure.Repositories;
public sealed class StudentEnrollmentRepository : IStudentEnrollmentRepository
{
    private readonly ApplicationDbContext _dbContext;

    public StudentEnrollmentRepository(
        ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<StudentEnrollmentDtl?> GetByIdAsync(
        long studentEnrollmentId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.StudentEnrollmentDtls
            .FirstOrDefaultAsync(
                x => x.StudentEnrollmentId == studentEnrollmentId,
                cancellationToken);
    }

    public async Task<IReadOnlyList<StudentEnrollmentDtl>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.StudentEnrollmentDtls
            .AsNoTracking()
            .OrderBy(x => x.StudentId)
            .ThenBy(x => x.SessionId)
            .ToListAsync(cancellationToken);
    }

    public async Task<StudentEnrollmentDtl?>
        GetByStudentAndSessionAsync(
            long studentId,
            long sessionId,
            CancellationToken cancellationToken = default)
    {
        return await _dbContext.StudentEnrollmentDtls
            .FirstOrDefaultAsync(
                x => x.StudentId == studentId &&
                     x.SessionId == sessionId,
                cancellationToken);
    }

    public async Task<bool> ExistsForStudentAndSessionAsync(
        long studentId,
        long sessionId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.StudentEnrollmentDtls
            .AnyAsync(
                x => x.StudentId == studentId &&
                     x.SessionId == sessionId,
                cancellationToken);
    }

    public async Task AddAsync(
        StudentEnrollmentDtl enrollment,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(enrollment);

        await _dbContext.StudentEnrollmentDtls
            .AddAsync(enrollment, cancellationToken);
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
