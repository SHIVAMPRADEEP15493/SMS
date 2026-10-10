using SMS.Domain.Entities;

namespace SMS.Application.Interfaces;

public interface IStudentEnrollmentRepository
{
    Task<StudentEnrollmentDtl?> GetByIdAsync(
        long studentEnrollmentId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<StudentEnrollmentDtl>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<StudentEnrollmentDtl?> GetByStudentAndSessionAsync(
        long studentId,
        long sessionId,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsForStudentAndSessionAsync(
        long studentId,
        long sessionId,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        StudentEnrollmentDtl enrollment,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);
}
