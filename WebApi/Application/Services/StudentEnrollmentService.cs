using SMS.Application.Common.Exceptions;
using SMS.Application.Features.StudentEnrollments.DTOs;
using SMS.Application.Interfaces;
using SMS.Domain.Entities;

namespace SMS.Application.Features.StudentEnrollments.Services;

public sealed class StudentEnrollmentService
{
    private static readonly string[] TerminalStatuses =
    [
        "Completed",
        "Transferred",
        "Withdrawn"
    ];

    private readonly IStudentEnrollmentRepository _repository;

    public StudentEnrollmentService(
        IStudentEnrollmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<StudentEnrollmentResponse> CreateAsync(
        CreateStudentEnrollmentRequest request,
        long? createdBy = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Friendly application-level check.
        // PostgreSQL's unique index remains the final authority.
        var alreadyExists =
            await _repository.ExistsForStudentAndSessionAsync(
                request.StudentId,
                request.SessionId,
                cancellationToken);

        if (alreadyExists)
        {
            throw new ConflictException(
                "This student already has an enrollment for the specified session.");
        }

        var enrollment = new StudentEnrollmentDtl(
            studentId: request.StudentId,
            schoolId: request.SchoolId,
            sessionId: request.SessionId,
            classId: request.ClassId,
            sectionId: request.SectionId,
            admissionDate: request.AdmissionDate,
            rollNumber: request.RollNumber,
            createdBy: createdBy);

        await _repository.AddAsync(
            enrollment,
            cancellationToken);

        await _repository.SaveChangesAsync(cancellationToken);

        return MapToResponse(enrollment);
    }

    public async Task<StudentEnrollmentResponse> GetByIdAsync(
        long studentEnrollmentId,
        CancellationToken cancellationToken = default)
    {
        var enrollment = await _repository.GetByIdAsync(
            studentEnrollmentId,
            cancellationToken);

        if (enrollment is null)
        {
            throw new NotFoundException(
                $"Student enrollment with ID {studentEnrollmentId} was not found.", studentEnrollmentId);
        }

        return MapToResponse(enrollment);
    }

    public async Task<IReadOnlyList<StudentEnrollmentResponse>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var enrollments = await _repository.GetAllAsync(
            cancellationToken);

        return enrollments
            .Select(MapToResponse)
            .ToArray();
    }

    public async Task<StudentEnrollmentResponse> UpdateAsync(
        long studentEnrollmentId,
        UpdateStudentEnrollmentRequest request,
        long? updatedBy = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var enrollment = await _repository.GetByIdAsync(
            studentEnrollmentId,
            cancellationToken);

        if (enrollment is null)
        {
            throw new NotFoundException(
                $"Student enrollment with ID {studentEnrollmentId} was not found.", studentEnrollmentId);
        }

        if (IsTerminalStatus(enrollment.Status))
        {
            throw new ConflictException(
                "A completed, transferred, or withdrawn enrollment cannot be updated.");
        }

        enrollment.UpdatePlacement(
            schoolId: request.SchoolId,
            classId: request.ClassId,
            sectionId: request.SectionId,
            rollNumber: request.RollNumber,
            updatedBy: updatedBy);

        await _repository.SaveChangesAsync(cancellationToken);

        return MapToResponse(enrollment);
    }

    public async Task<StudentEnrollmentResponse> UpdateStatusAsync(
        long studentEnrollmentId,
        UpdateStudentEnrollmentStatusRequest request,
        long? updatedBy = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var enrollment = await _repository.GetByIdAsync(
            studentEnrollmentId,
            cancellationToken);

        if (enrollment is null)
        {
            throw new NotFoundException(
                $"Student enrollment with ID {studentEnrollmentId} was not found.", studentEnrollmentId);
        }

        var requestedStatus = NormalizeStatus(request.Status);

        // Allow an idempotent request to set the current terminal status.
        if (string.Equals(
                enrollment.Status,
                requestedStatus,
                StringComparison.OrdinalIgnoreCase))
        {
            return MapToResponse(enrollment);
        }

        if (IsTerminalStatus(enrollment.Status))
        {
            throw new ConflictException(
                "A completed, transferred, or withdrawn enrollment cannot change status.");
        }

        enrollment.UpdateStatus(
            status: requestedStatus,
            updatedBy: updatedBy);

        await _repository.SaveChangesAsync(cancellationToken);

        return MapToResponse(enrollment);
    }

    private static bool IsTerminalStatus(string status)
    {
        return TerminalStatuses.Contains(
            status,
            StringComparer.OrdinalIgnoreCase);
    }

    private static string NormalizeStatus(string? status)
    {
        var allowedStatuses = new[]
        {
            "Active",
            "Completed",
            "Transferred",
            "Withdrawn"
        };

        var normalized = allowedStatuses.FirstOrDefault(
            allowed => string.Equals(
                allowed,
                status?.Trim(),
                StringComparison.OrdinalIgnoreCase));

        if (normalized is null)
        {
            throw new ArgumentException(
                "Status must be Active, Completed, Transferred, or Withdrawn.",
                nameof(status));
        }

        return normalized;
    }

    private static StudentEnrollmentResponse MapToResponse(
        StudentEnrollmentDtl enrollment)
    {
        return new StudentEnrollmentResponse
        {
            StudentEnrollmentId = enrollment.StudentEnrollmentId,
            StudentId = enrollment.StudentId,
            SchoolId = enrollment.SchoolId,
            SessionId = enrollment.SessionId,
            ClassId = enrollment.ClassId,
            SectionId = enrollment.SectionId,
            AdmissionDate = enrollment.AdmissionDate,
            RollNumber = enrollment.RollNumber,
            Status = enrollment.Status,
            CreatedAt = enrollment.CreatedAt,
            CreatedBy = enrollment.CreatedBy,
            UpdatedAt = enrollment.UpdatedAt,
            UpdatedBy = enrollment.UpdatedBy
        };
    }
}