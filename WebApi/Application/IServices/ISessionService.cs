using System;
using System.Collections.Generic;
using System.Text;
using  SMS.Application.DTOs;

namespace SMS.Application.IServices;
public interface ISessionService
{
    Task<SessionMstResponse> CreateAsync(CreateSessionMstRequest request, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SessionMstResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<SessionMstResponse> GetByIdAsync(long sessionId, CancellationToken cancellationToken = default);
    Task<SessionMstResponse> UpdateAsync(long sessionId, UpdateSessionMstRequest request, CancellationToken cancellationToken = default);
    Task<SessionMstResponse> ActivateAsync(long sessionId, CancellationToken cancellationToken = default);
    Task<SessionMstResponse> DeactivateAsync(long sessionId, CancellationToken cancellationToken = default);
    Task<SessionMstResponse> SetCurrentAsync(long sessionId, CancellationToken cancellationToken = default);
    Task<SessionMstResponse> GetCurrentAsync(CancellationToken cancellationToken = default);
}
