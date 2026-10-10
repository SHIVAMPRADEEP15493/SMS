using SMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Application.Interfaces;
public interface IClassRepository
{
    Task<ClassMst?> GetByIdAsync(
        long classId,
        CancellationToken cancellationToken = default);
    Task<ClassMst?> GetByCodeAsync(
        string classCode,
        CancellationToken cancellationToken = default);
    Task<bool> ExistsByCodeAsync(
        string classCode,
        CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(
        string className,
        CancellationToken cancellationToken = default);
    Task<bool> ExistsByDisplayOrderAsync(
        int displayOrder,
        CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ClassMst>> GetAllAsync(
        CancellationToken cancellationToken = default);
    Task AddAsync(
        ClassMst classMst,
        CancellationToken cancellationToken = default);
    void Remove(ClassMst classMst);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(
    string className,
    long excludeClassId,
    CancellationToken cancellationToken = default);

    Task<bool> ExistsByDisplayOrderAsync(
        int displayOrder,
        long excludeClassId,
        CancellationToken cancellationToken = default);
}
