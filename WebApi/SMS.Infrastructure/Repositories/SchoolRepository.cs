using Microsoft.EntityFrameworkCore;
using SMS.Application.SchoolsMst.Interfaces;
using SMS.Domain.Entities;
using SMS.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Infrastructure.Repositories;
public class SchoolRepository : ISchoolRepository
{
    private readonly ApplicationDbContext _context;
    public SchoolRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<SMS_School_Mst?> GetByIdAsync( Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.SchoolsMst
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }
    public async Task<IReadOnlyList<SMS_School_Mst>> GetAllAsync( CancellationToken cancellationToken = default)
    {
        return await _context.SchoolsMst.AsNoTracking()
            .OrderBy(x => x.SchoolName)
            .ToListAsync(cancellationToken);
    }
    public async Task<bool> ExistsByCodeAsync( string code, CancellationToken cancellationToken = default)
    {
        return await _context.SchoolsMst.AnyAsync(x => x.SchoolCode == code, cancellationToken);
    }
    public async Task AddAsync( SMS_School_Mst school, CancellationToken cancellationToken = default)
    {
        await _context.SchoolsMst.AddAsync( school, cancellationToken);
    }
    public void Update(SMS_School_Mst school)
    {
        _context.SchoolsMst.Update(school);
    }
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
