using Microsoft.EntityFrameworkCore;
using SMS.Application.Interfaces;
using SMS.Domain.Entities;
using SMS.Infrastructure.Persistence;
using System.Data;

namespace SMS.Infrastructure.Repositories;
public sealed class SessionRepository : ISessionRepository
{
    private readonly ApplicationDbContext _dbContext;
    public SessionRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<SessionMst?> GetByIdAsync(long sessionId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.SessionMst.FirstOrDefaultAsync(x => x.SessionId == sessionId, cancellationToken);
    }
    public async Task<SessionMst?> GetByCodeAsync(string sessionCode, CancellationToken cancellationToken = default)
    {
        return await _dbContext.SessionMst.FirstOrDefaultAsync(x => x.SessionCode == sessionCode, cancellationToken);
    }
    public async Task<IReadOnlyList<SessionMst>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SessionMst.AsNoTracking().OrderByDescending(x => x.StartDate).ToListAsync(cancellationToken);
    }
    public async Task<SessionMst?> GetCurrentAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SessionMst.FirstOrDefaultAsync(x => x.IsCurrent, cancellationToken);
    }
    public async Task AddAsync(SessionMst session, CancellationToken cancellationToken = default)
    {
        await _dbContext.SessionMst.AddAsync(session, cancellationToken);
    }
    public void Update(SessionMst session)
    {
        _dbContext.SessionMst.Update(session);
    }
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
    public async Task<SessionMst?> SetCurrentAsync(long sessionId, long? updatedBy, DateTimeOffset updatedAt, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        try
        {
            var targetSession = await _dbContext.SessionMst.FirstOrDefaultAsync(x => x.SessionId == sessionId, cancellationToken);
            if (targetSession is null)
            {
                return null;
            }
            var currentSession = await _dbContext.SessionMst.FirstOrDefaultAsync(x => x.IsCurrent, cancellationToken);
            if (currentSession?.SessionId == sessionId)
            {
                await transaction.CommitAsync(cancellationToken);
                return targetSession;
            }
            if (currentSession is not null)
            {
                currentSession.RemoveCurrent(updatedBy, updatedAt);
                await _dbContext.SaveChangesAsync(cancellationToken);  
            }
            targetSession.SetCurrent(updatedBy, updatedAt);
            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return targetSession;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
