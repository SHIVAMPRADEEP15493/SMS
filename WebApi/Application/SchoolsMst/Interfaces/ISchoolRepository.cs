using SMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Application.SchoolsMst.Interfaces;
public interface ISchoolRepository
{
    Task<SMS_School_Mst?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SMS_School_Mst>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default);

    Task AddAsync(SMS_School_Mst school, CancellationToken cancellationToken = default);

    void Update(SMS_School_Mst school);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
