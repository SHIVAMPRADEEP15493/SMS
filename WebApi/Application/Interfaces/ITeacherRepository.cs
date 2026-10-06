using SMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Application.Interfaces;
public interface ITeacherRepository
{
    Task<TeacherMst?> GetByIdAsync(long teacherId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TeacherMst>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsByTeacherCodeAsync(long schoolId, string teacherCode, long? excludeTeacherId = null, CancellationToken cancellationToken = default);
    Task<bool> ExistsByEmployeeCodeAsync(long schoolId, string employeeCode, long? excludeTeacherId = null, CancellationToken cancellationToken = default);
    Task AddAsync(TeacherMst teacher, CancellationToken cancellationToken = default);
    Task<bool> SchoolExistsAsync(long schoolId, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
