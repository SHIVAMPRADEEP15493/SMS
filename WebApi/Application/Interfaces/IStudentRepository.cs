using SMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Application.Interfaces;
public interface IStudentRepository
{
    Task<StudentMst?> GetByIdAsync(
        long studentId,
        CancellationToken cancellationToken = default);

    Task<StudentMst?> GetByStudentCodeAsync(
        string studentCode,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByStudentCodeAsync(
        string studentCode,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<StudentMst>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task AddAsync(
        StudentMst student,
        CancellationToken cancellationToken = default);

    void Update(StudentMst student);
    void Remove(StudentMst student);
    Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default);
}
