using SMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Application.Interfaces;
public interface ISchoolRepository
{
    Task<SchoolMst?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SchoolMst>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task AddAsync(SchoolMst school, CancellationToken cancellationToken = default);
    void Update(SchoolMst school);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
