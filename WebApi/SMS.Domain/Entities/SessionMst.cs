using SMS.Domain.Exceptions;

namespace SMS.Domain.Entities;
public sealed class SessionMst
{
    private SessionMst()
    {
    }
    public long SessionId { get; private set; }
    public string SessionCode { get; private set; } = null!;
    public string SessionName { get; private set; } = null!;
    public DateOnly StartDate { get; private set; }
    public DateOnly EndDate { get; private set; }
    public bool IsCurrent { get; private set; }
    public bool IsActive { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public long? CreatedBy { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }
    public long? UpdatedBy { get; private set; }
    public static SessionMst Create(
        string sessionCode,
        string sessionName,
        DateOnly startDate,
        DateOnly endDate,
        bool isCurrent,
        long? createdBy,
        DateTimeOffset createdAt)
    {
        if (startDate >= endDate)
        {
            throw new ArgumentException(
                "Session start date must be earlier than end date.",
                nameof(startDate));
        }

        return new SessionMst
        {
            SessionCode = sessionCode,
            SessionName = sessionName,
            StartDate = startDate,
            EndDate = endDate,
            IsCurrent = isCurrent,
            IsActive = true,
            CreatedAt = createdAt,
            CreatedBy = createdBy
        };
    }
    public void UpdateDetails(
        string sessionName,
        DateOnly startDate,
        DateOnly endDate,
        long? updatedBy,
        DateTimeOffset updatedAt)
    {
        if (startDate >= endDate)
        {
            throw new ArgumentException(
                "Session start date must be earlier than end date.");
        }

        SessionName = sessionName;
        StartDate = startDate;
        EndDate = endDate;

        SetUpdatedAudit(updatedBy, updatedAt);
    }
    public void Activate(long? updatedBy, DateTimeOffset updatedAt)
    {
        IsActive = true;
        SetUpdatedAudit(updatedBy, updatedAt);
    }
    public void Deactivate(long? updatedBy, DateTimeOffset updatedAt)
    {
        if (IsCurrent)
        {
            throw new DomainRuleViolationException("The current session cannot be deactivated.");
        }

        IsActive = false;
        SetUpdatedAudit(updatedBy, updatedAt);
    }
    public void SetCurrent(long? updatedBy, DateTimeOffset updatedAt)
    {
        if (!IsActive)
        {
            throw new DomainRuleViolationException("An inactive session cannot be set as current.");
        }

        IsCurrent = true;
        SetUpdatedAudit(updatedBy, updatedAt);
    }
    public void RemoveCurrent(long? updatedBy, DateTimeOffset updatedAt)
    {
        IsCurrent = false;
        SetUpdatedAudit(updatedBy, updatedAt);
    }
    private void SetUpdatedAudit(long? updatedBy, DateTimeOffset updatedAt)
    {
        UpdatedBy = updatedBy;
        UpdatedAt = updatedAt;
    }
}
