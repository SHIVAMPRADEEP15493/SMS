using SMS.Application.Common.Exceptions;
using SMS.Application.DTOs;
using SMS.Application.Interfaces;
using SMS.Domain.Entities;
using SMS.Domain.Enums;


namespace SMS.Application.Services;
public sealed class StudentService
{
    private readonly IStudentRepository _studentRepository;

    public StudentService(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public async Task<StudentMstResponse?> GetByIdAsync(
        long studentId,
        CancellationToken cancellationToken = default)
    {
        var student = await _studentRepository.GetByIdAsync(
            studentId,
            cancellationToken);

        return student is null
            ? null
            : MapToResponse(student);
    }

    public async Task<IReadOnlyList<StudentMstResponse>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var students = await _studentRepository.GetAllAsync(
            cancellationToken);

        return students
            .Select(MapToResponse)
            .ToList();
    }

    public async Task<StudentMstResponse> CreateAsync(
        CreateStudentMstRequest request,
        CancellationToken cancellationToken = default)
    {
        var studentCodeExists =
            await _studentRepository.ExistsByStudentCodeAsync(
                request.StudentCode.Trim(),
                cancellationToken);

        if (studentCodeExists)
        {
            throw new ConflictException(
                "Student code already exists.");
        }

        var student = new StudentMst(
            studentCode: request.StudentCode.Trim(),
            admissionNumber: request.AdmissionNumber.Trim(),
            admissionDate: request.AdmissionDate,
            admittedSessionId: request.AdmittedSessionId,
            firstName: request.FirstName.Trim(),
            middleName: NormalizeOptional(request.MiddleName),
            lastName: NormalizeOptional(request.LastName),
            dateOfBirth: request.DateOfBirth,
            gender: request.Gender.Trim(),
            mobileNumber: NormalizeOptional(request.MobileNumber),
            email: NormalizeOptional(request.Email),
            address: NormalizeOptional(request.Address),
            fatherName: NormalizeOptional(request.FatherName),
            motherName: NormalizeOptional(request.MotherName),
            guardianName: NormalizeOptional(request.GuardianName),
            guardianMobileNumber: NormalizeOptional(request.GuardianMobileNumber),
            createdBy: request.CreatedBy);

        await _studentRepository.AddAsync(
            student,
            cancellationToken);

        await _studentRepository.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(student);
    }

    public async Task<StudentMstResponse?> UpdateAsync(
        long studentId,
        UpdateStudentMstRequest request,
        CancellationToken cancellationToken = default)
    {
        var student = await _studentRepository.GetByIdAsync(
            studentId,
            cancellationToken);

        if (student is null)
        {
            return null;
        }

        student.Update(
            firstName: request.FirstName.Trim(),
            middleName: NormalizeOptional(request.MiddleName),
            lastName: NormalizeOptional(request.LastName),
            dateOfBirth: request.DateOfBirth,
            gender: request.Gender.Trim(),
            mobileNumber: NormalizeOptional(request.MobileNumber),
            email: NormalizeOptional(request.Email),
            address: NormalizeOptional(request.Address),
            fatherName: NormalizeOptional(request.FatherName),
            motherName: NormalizeOptional(request.MotherName),
            guardianName: NormalizeOptional(request.GuardianName),
            guardianMobileNumber: NormalizeOptional(request.GuardianMobileNumber),
            updatedBy: request.UpdatedBy);

        _studentRepository.Update(student);

        await _studentRepository.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(student);
    }

    public async Task<bool> UpdateStatusAsync(
        long studentId,
        string status,
        long? updatedBy = null,
        CancellationToken cancellationToken = default)
    {
        var student = await _studentRepository.GetByIdAsync(
            studentId,
            cancellationToken);

        if (student is null)
        {
            return false;
        }

        if (!Enum.TryParse<StudentStatus>(
                status,
                ignoreCase: true,
                out var newStatus))
        {
            throw new NotFoundException("Invalid student status. Allowed values are Active, Completed, Transferred, and Withdrawn.", string.Empty);
        }

        ValidateStatusTransition(
            student.CurrentStatus,
            newStatus);

        student.UpdateStatus(
            newStatus,
            updatedBy);

        _studentRepository.Update(student);

        await _studentRepository.SaveChangesAsync(
            cancellationToken);

        return true;
    }

    private static void ValidateStatusTransition(
        StudentStatus currentStatus,
        StudentStatus newStatus)
    {
        if (currentStatus == newStatus)
        {
            return;
        }

        if (currentStatus != StudentStatus.Active)
        {
            throw new ConflictException(
                $"Student status cannot be changed from " +
                $"{currentStatus} to {newStatus}.");
        }
    }

    private static string? NormalizeOptional(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }

    private static StudentMstResponse MapToResponse(
        StudentMst student)
    {
        return new StudentMstResponse
        {
            StudentId = student.StudentId,
            StudentCode = student.StudentCode,
            AdmissionNumber = student.AdmissionNumber,
            AdmissionDate = student.AdmissionDate,
            AdmittedSessionId = student.AdmittedSessionId,
            FirstName = student.FirstName,
            MiddleName = student.MiddleName,
            LastName = student.LastName,
            DateOfBirth = student.DateOfBirth,
            Gender = student.Gender,
            MobileNumber = student.MobileNumber,
            Email = student.Email,
            Address = student.Address,
            FatherName = student.FatherName,
            MotherName = student.MotherName,
            GuardianName = student.GuardianName,
            CurrentStatus = student.CurrentStatus.ToString(),
            IsActive = student.IsActive,
            CreatedAt = student.CreatedAt,
            CreatedBy = student.CreatedBy,
            UpdatedAt = student.UpdatedAt,
            UpdatedBy = student.UpdatedBy
            //,Version = student.Version
        };
    }
}
