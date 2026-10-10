using SMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Application.Interfaces;
public interface ISectionRepository
{
    Task<SectionMst?> GetByIdAsync(long sectionId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SectionMst>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(SectionMst section, CancellationToken cancellationToken = default);
    Task RemoveAsync(SectionMst section, CancellationToken cancellationToken = default);
    Task<bool> ExistsByCodeAsync(string sectionCode, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(string sectionName, CancellationToken cancellationToken = default);
    Task<bool> ExistsByDisplayOrderAsync(int displayOrder, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(string sectionName, long excludeSectionId, CancellationToken cancellationToken = default);
    Task<bool> ExistsByDisplayOrderAsync(int displayOrder, long excludeSectionId, CancellationToken cancellationToken = default);
}
