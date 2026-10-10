using Microsoft.EntityFrameworkCore;
using SMS.Application.Interfaces;
using SMS.Domain.Entities;
using SMS.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Infrastructure.Repositories;
public sealed class SectionRepository : ISectionRepository
{
    private readonly ApplicationDbContext _context;
    public SectionRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<SectionMst?> GetByIdAsync(long sectionId, CancellationToken cancellationToken = default)
    {
        return await _context.SectionMst
            .FirstOrDefaultAsync(
                x => x.SectionId == sectionId,
                cancellationToken);
    }
    public async Task<IReadOnlyList<SectionMst>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SectionMst
            .AsNoTracking()
            .OrderBy(x => x.DisplayOrder)
            .ThenBy(x => x.SectionId)
            .ToListAsync(cancellationToken);
    }
    public async Task AddAsync(SectionMst section, CancellationToken cancellationToken = default)
    {
        await _context.SectionMst.AddAsync(
            section,
            cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
    public async Task RemoveAsync(SectionMst section, CancellationToken cancellationToken = default)
    {
        _context.SectionMst.Remove(section);
        await _context.SaveChangesAsync(cancellationToken);
    }
    public async Task<bool> ExistsByCodeAsync(string sectionCode, CancellationToken cancellationToken = default)
    {
        return await _context.SectionMst
            .AsNoTracking()
            .AnyAsync(
                x => x.SectionCode == sectionCode,
                cancellationToken);
    }
    public async Task<bool> ExistsByNameAsync(string sectionName, CancellationToken cancellationToken = default)
    {
        return await _context.SectionMst  
            .AsNoTracking()
            .AnyAsync(
                x => x.SectionName == sectionName,
                cancellationToken);
    }
    public async Task<bool> ExistsByDisplayOrderAsync(int displayOrder, CancellationToken cancellationToken = default)
    {
        return await _context.SectionMst
            .AsNoTracking()
            .AnyAsync(
                x => x.DisplayOrder == displayOrder,
                cancellationToken);
    }
    public async Task<bool> ExistsByNameAsync(string sectionName, long excludeSectionId, CancellationToken cancellationToken = default)
    {
        return await _context.SectionMst
            .AsNoTracking()
            .AnyAsync(
                x =>
                    x.SectionName == sectionName &&
                    x.SectionId != excludeSectionId,
                cancellationToken);
    }
    public async Task<bool> ExistsByDisplayOrderAsync(int displayOrder, long excludeSectionId, CancellationToken cancellationToken = default)
    {
        return await _context.SectionMst
            .AsNoTracking()
            .AnyAsync(
                x =>
                    x.DisplayOrder == displayOrder &&
                    x.SectionId != excludeSectionId,
                cancellationToken);
    }
}
