namespace SMS.Application.Features.StudentEnrollments.DTOs;

public sealed class UpdateStudentEnrollmentRequest
{
    public long SchoolId { get; init; }
    public long ClassId { get; init; }
    public long SectionId { get; init; }
    public string RollNumber { get; init; } = string.Empty;
}
