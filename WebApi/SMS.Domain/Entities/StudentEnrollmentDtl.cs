namespace SMS.Domain.Entities;

public class StudentEnrollmentDtl
{
    public long StudentEnrollmentId { get; private set; }

    public long StudentId { get; private set; }

    public long SchoolId { get; private set; }

    public long SessionId { get; private set; }

    public long ClassId { get; private set; }

    public long SectionId { get; private set; }

    // Date the student entered this particular enrollment.
    public DateOnly AdmissionDate { get; private set; }

    public string RollNumber { get; private set; } = string.Empty;

    public string Status { get; private set; } = "Active";

    public DateTimeOffset CreatedAt { get; private set; }

    public long? CreatedBy { get; private set; }

    public DateTimeOffset? UpdatedAt { get; private set; }

    public long? UpdatedBy { get; private set; }

    // Required by EF Core.
    private StudentEnrollmentDtl()
    {
    }

    public StudentEnrollmentDtl(
        long studentId,
        long schoolId,
        long sessionId,
        long classId,
        long sectionId,
        DateOnly admissionDate,
        string rollNumber,
        long? createdBy = null)
    {
        if (studentId <= 0)
            throw new ArgumentOutOfRangeException(nameof(studentId));

        if (schoolId <= 0)
            throw new ArgumentOutOfRangeException(nameof(schoolId));

        if (sessionId <= 0)
            throw new ArgumentOutOfRangeException(nameof(sessionId));

        if (classId <= 0)
            throw new ArgumentOutOfRangeException(nameof(classId));

        if (sectionId <= 0)
            throw new ArgumentOutOfRangeException(nameof(sectionId));

        if (string.IsNullOrWhiteSpace(rollNumber))
            throw new ArgumentException(
                "Roll number is required.",
                nameof(rollNumber));

        StudentId = studentId;
        SchoolId = schoolId;
        SessionId = sessionId;
        ClassId = classId;
        SectionId = sectionId;
        AdmissionDate = admissionDate;
        RollNumber = rollNumber.Trim();
        Status = "Active";
        CreatedAt = DateTimeOffset.UtcNow;
        CreatedBy = createdBy;
    }

    public void UpdatePlacement(
        long schoolId,
        long classId,
        long sectionId,
        string rollNumber,
        long? updatedBy = null)
    {
        if (schoolId <= 0)
            throw new ArgumentOutOfRangeException(nameof(schoolId));

        if (classId <= 0)
            throw new ArgumentOutOfRangeException(nameof(classId));

        if (sectionId <= 0)
            throw new ArgumentOutOfRangeException(nameof(sectionId));

        if (string.IsNullOrWhiteSpace(rollNumber))
            throw new ArgumentException(
                "Roll number is required.",
                nameof(rollNumber));

        SchoolId = schoolId;
        ClassId = classId;
        SectionId = sectionId;
        RollNumber = rollNumber.Trim();
        UpdatedAt = DateTimeOffset.UtcNow;
        UpdatedBy = updatedBy;
    }

    public void UpdateStatus(
        string status,
        long? updatedBy = null)
    {
        var allowedStatuses = new[] { "Active", "Completed", "Transferred", "Withdrawn" };

        if (string.IsNullOrWhiteSpace(status) ||
            !allowedStatuses.Contains(status, StringComparer.OrdinalIgnoreCase))
        {
            throw new ArgumentException(
                "Enrollment status is invalid.",
                nameof(status));
        }

        Status = allowedStatuses.First(
            value => string.Equals(
                value,
                status,
                StringComparison.OrdinalIgnoreCase));

        UpdatedAt = DateTimeOffset.UtcNow;
        UpdatedBy = updatedBy;
    }
}
