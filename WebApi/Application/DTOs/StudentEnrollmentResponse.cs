namespace SMS.Application.Features.StudentEnrollments.DTOs;

public sealed class StudentEnrollmentResponse
{
    public long StudentEnrollmentId { get; init; }

    public long StudentId { get; init; }

    public long SchoolId { get; init; }

    public long SessionId { get; init; }

    public long ClassId { get; init; }

    public long SectionId { get; init; }

    public DateOnly AdmissionDate { get; init; }

    public string RollNumber { get; init; } = string.Empty;

    public string Status { get; init; } = string.Empty;

    public DateTimeOffset CreatedAt { get; init; }

    public long? CreatedBy { get; init; }

    public DateTimeOffset? UpdatedAt { get; init; }

    public long? UpdatedBy { get; init; }
}