using Microsoft.EntityFrameworkCore;
using Npgsql;
using SMS.Application.Interfaces;
using SMS.Domain.Entities;
using SMS.Infrastructure.Persistence;
using SMS.Infrastructure.Persistence.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Infrastructure.Repositories;
public sealed class ClassRepository : IClassRepository
{
    private readonly ApplicationDbContext _dbContext;
    public ClassRepository(
        ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<ClassMst?> GetByIdAsync(
        long classId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.ClassMst
            .FirstOrDefaultAsync(
                x => x.ClassId == classId,
                cancellationToken);
    }
    public async Task<ClassMst?> GetByCodeAsync(
        string classCode,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.ClassMst
            .FirstOrDefaultAsync(
                x => x.ClassCode == classCode,
                cancellationToken);
    }
    public async Task<bool> ExistsByCodeAsync(
        string classCode,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.ClassMst
            .AnyAsync(
                x => x.ClassCode == classCode,
                cancellationToken);
    }
    public async Task<bool> ExistsByNameAsync(
        string className,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.ClassMst
            .AnyAsync(
                x => x.ClassName == className,
                cancellationToken);
    }
    public async Task<bool> ExistsByDisplayOrderAsync(
        int displayOrder,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.ClassMst
            .AnyAsync(
                x => x.DisplayOrder == displayOrder,
                cancellationToken);
    }
    public async Task<IReadOnlyList<ClassMst>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.ClassMst
            .AsNoTracking()
            .OrderBy(x => x.DisplayOrder)
            .ToListAsync(cancellationToken);
    }
    public async Task AddAsync(
        ClassMst classMst,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.ClassMst.AddAsync(
            classMst,
            cancellationToken);
    }
    public void Remove(ClassMst classMst)
    {
        _dbContext.ClassMst.Remove(classMst);
    }
    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbContext.SaveChangesAsync(
                cancellationToken);
        }
        catch (PostgresException exception)
        {
            throw PostgresExceptionTranslator
                .Translate(exception);
        }
    }
    public async Task<bool> ExistsByNameAsync(
    string className,
    long excludeClassId,
    CancellationToken cancellationToken = default)
    {
        return await _dbContext.ClassMst
            .AnyAsync(
                x => x.ClassName == className &&
                     x.ClassId != excludeClassId,
                cancellationToken);
    }

    public async Task<bool> ExistsByDisplayOrderAsync(
    int displayOrder,
    long excludeClassId,
    CancellationToken cancellationToken = default)
    {
        return await _dbContext.ClassMst
            .AnyAsync(
                x => x.DisplayOrder == displayOrder &&
                     x.ClassId != excludeClassId,
                cancellationToken);
    }
}
