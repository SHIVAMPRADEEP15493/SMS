using Microsoft.EntityFrameworkCore;
using SMS.Application.Interfaces;
using SMS.Domain.Entities;
using SMS.Infrastructure.Persistence;

namespace SMS.Infrastructure.Repositories;
public class SchoolRepository : ISchoolRepository
{
    private readonly ApplicationDbContext _context;
    public SchoolRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<SchoolMst?> GetByIdAsync( long id, CancellationToken cancellationToken = default)
    {
        return await _context.SchoolsMst.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
    public async Task<IReadOnlyList<SchoolMst>> GetAllAsync( CancellationToken cancellationToken = default)
    {
        return await _context.SchoolsMst.AsNoTracking().OrderBy(x => x.SchoolName).ToListAsync(cancellationToken);
    }
    public async Task<bool> ExistsByCodeAsync( string code, CancellationToken cancellationToken = default)
    {
        return await _context.SchoolsMst.AnyAsync(x => x.SchoolCode == code, cancellationToken);
    }
    public async Task AddAsync( SchoolMst school, CancellationToken cancellationToken = default)
    {
        await _context.SchoolsMst.AddAsync( school, cancellationToken);
    }
    public void Update(SchoolMst school)
    {
        _context.SchoolsMst.Update(school);
    }
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
