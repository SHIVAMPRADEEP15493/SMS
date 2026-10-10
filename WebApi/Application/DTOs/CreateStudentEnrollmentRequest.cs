
namespace SMS.Application.Features.StudentEnrollments.DTOs;

public sealed class CreateStudentEnrollmentRequest
{
    public long StudentId { get; init; }
    public long SchoolId { get; init; }
    public long SessionId { get; init; }
    public long ClassId { get; init; }
    public long SectionId { get; init; }
    public DateOnly AdmissionDate { get; init; }
    public string RollNumber { get; init; } = string.Empty;
}
