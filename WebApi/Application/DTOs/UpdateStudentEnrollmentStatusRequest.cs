namespace SMS.Application.Features.StudentEnrollments.DTOs;

public sealed class UpdateStudentEnrollmentStatusRequest
{
    public string Status { get; init; } = string.Empty;
}