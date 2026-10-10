using SMS.Domain.Entities;
using SMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using SMS.Application.Interfaces;

namespace SMS.Infrastructure.Repositories;
public sealed class StudentRepository : IStudentRepository
{
    private readonly ApplicationDbContext _dbContext;
    public StudentRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<StudentMst?> GetByIdAsync(
        long studentId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.StudentMst
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.StudentId == studentId,
                cancellationToken);
    }

    public async Task<StudentMst?> GetByStudentCodeAsync(
        string studentCode,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.StudentMst
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.StudentCode == studentCode,
                cancellationToken);
    }

    public async Task<bool> ExistsByStudentCodeAsync(
        string studentCode,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.StudentMst
            .AsNoTracking()
            .AnyAsync(
                x => x.StudentCode == studentCode,
                cancellationToken);
    }

    public async Task<IReadOnlyList<StudentMst>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.StudentMst
            .AsNoTracking()
            .OrderBy(x => x.StudentId)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        StudentMst student,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.StudentMst.AddAsync(
            student,
            cancellationToken);
    }

    public void Update(StudentMst student)
    {
        _dbContext.StudentMst.Update(student);
    }

    public void Remove(StudentMst student)
    {
        _dbContext.StudentMst.Remove(student);
    }

    public async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(
            cancellationToken);
    }
}
