using SMS.Application.Common.Exceptions;
using SMS.Application.DTOs;
using SMS.Application.Interfaces;
using SMS.Domain.Entities;

namespace SMS.Application.Services;
public sealed class SessionService
{
    private readonly ISessionRepository _sessionRepository;
    public SessionService(ISessionRepository sessionRepository)
    {
        _sessionRepository = sessionRepository;
    }
    public async Task<SessionMstResponse> CreateAsync(CreateSessionMstRequest request, CancellationToken cancellationToken = default)
    {
        var existingSession = await _sessionRepository.GetByCodeAsync(request.SessionCode, cancellationToken);
        if (existingSession is not null)
        {
            throw new ConflictException($"Session code '{request.SessionCode}' already exists.");
        }
        var session = SessionMst.Create(
            request.SessionCode,
            request.SessionName,
            request.StartDate,
            request.EndDate,
            request.IsCurrent,
            createdBy: null,
            DateTimeOffset.UtcNow);

        await _sessionRepository.AddAsync(session, cancellationToken);
        await _sessionRepository.SaveChangesAsync(cancellationToken);
        return MapToResponse(session);
    }
    public async Task<IReadOnlyList<SessionMstResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var sessions = await _sessionRepository.GetAllAsync(cancellationToken);
        return sessions.Select(MapToResponse).ToList();
    }
    public async Task<SessionMstResponse> GetByIdAsync(long sessionId, CancellationToken cancellationToken = default)
    {
        var session = await _sessionRepository.GetByIdAsync(sessionId, cancellationToken);
        if (session is null)
        {
            throw new NotFoundException($"Session with id '{sessionId}' was not found.", sessionId);
        }
        return MapToResponse(session);
    }
    public async Task<SessionMstResponse> UpdateAsync(long sessionId, UpdateSessionMstRequest request, CancellationToken cancellationToken = default)
    {
        var session = await _sessionRepository.GetByIdAsync(sessionId, cancellationToken);
        if (session is null)
        {
            throw new NotFoundException($"Session with id '{sessionId}' was not found.", sessionId);
        }
        session.UpdateDetails(
            request.SessionName,
            request.StartDate,
            request.EndDate,
            updatedBy: null,
            DateTimeOffset.UtcNow);

        await _sessionRepository.SaveChangesAsync(cancellationToken);
        return MapToResponse(session);
    }
    public async Task<SessionMstResponse> ActivateAsync(long sessionId, CancellationToken cancellationToken = default)
    {
        var session = await _sessionRepository.GetByIdAsync(sessionId, cancellationToken);
        if (session is null)
        {
            throw new NotFoundException($"Session with id '{sessionId}' was not found.", sessionId);
        }
        session.Activate(updatedBy: null, DateTimeOffset.UtcNow);
        await _sessionRepository.SaveChangesAsync(cancellationToken);
        return MapToResponse(session);
    }
    public async Task<SessionMstResponse> DeactivateAsync(long sessionId, CancellationToken cancellationToken = default)
    {
        var session = await _sessionRepository.GetByIdAsync(sessionId, cancellationToken); 
        if (session is null)
        {
            throw new NotFoundException($"Session with id '{sessionId}' was not found.", sessionId);
        }

        session.Deactivate(updatedBy: null, DateTimeOffset.UtcNow);
        await _sessionRepository.SaveChangesAsync(cancellationToken);
        return MapToResponse(session);
    }
    public async Task<SessionMstResponse> SetCurrentAsync(long sessionId, CancellationToken cancellationToken = default)
    {
        /*
        var session =
            await _sessionRepository.GetByIdAsync(
                sessionId,
                cancellationToken);

        if (session is null)
        {
            throw new NotFoundException(
                $"Session with id '{sessionId}' was not found.",
                sessionId);
        }

        var currentSession =
            await _sessionRepository.GetCurrentAsync(
                cancellationToken);

        if (currentSession is not null &&
            currentSession.SessionId != sessionId)
        {
            currentSession.RemoveCurrent(
                updatedBy: null,
                DateTimeOffset.UtcNow);
        }

        session.SetCurrent(
            updatedBy: null,
            DateTimeOffset.UtcNow);

        await _sessionRepository.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(session);
        */

        var session = await _sessionRepository.SetCurrentAsync(sessionId, updatedBy: null, DateTimeOffset.UtcNow, cancellationToken);
        if (session is null)
        {
            throw new NotFoundException($"Session with id '{sessionId}' was not found.", sessionId);
        }
        return MapToResponse(session);
    }
    public async Task<SessionMstResponse> GetCurrentAsync(CancellationToken cancellationToken = default)
    {
        var session = await _sessionRepository.GetCurrentAsync(cancellationToken);
        if (session is null)
        {
            throw new NotFoundException("No current session is configured.", null);
        }
        return MapToResponse(session);
    }
    private static SessionMstResponse MapToResponse(SessionMst session)
    {
        return new SessionMstResponse
        {
            SessionId = session.SessionId,
            SessionCode = session.SessionCode,
            SessionName = session.SessionName,
            StartDate = session.StartDate,
            EndDate = session.EndDate,
            IsCurrent = session.IsCurrent,
            IsActive = session.IsActive,
            CreatedAt = session.CreatedAt,
            CreatedBy = session.CreatedBy,
            UpdatedAt = session.UpdatedAt,
            UpdatedBy = session.UpdatedBy
        };
    }
}
