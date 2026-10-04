using SMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Collections.Specialized.BitVector32;

namespace SMS.Application.Interfaces;
public interface ISessionRepository
{
    Task<SessionMst?> GetByIdAsync(long sessionId, CancellationToken cancellationToken = default);
    Task<SessionMst?> GetByCodeAsync(string sessionCode, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SessionMst>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<SessionMst?> GetCurrentAsync(CancellationToken cancellationToken = default);
    Task AddAsync(SessionMst session, CancellationToken cancellationToken = default);
    void Update(SessionMst session);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<SessionMst?> SetCurrentAsync(long sessionId, long? updatedBy, DateTimeOffset updatedAt, CancellationToken cancellationToken = default);
}
