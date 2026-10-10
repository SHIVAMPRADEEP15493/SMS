using SMS.Application.Common.Exceptions;
using SMS.Application.Interfaces;
using SMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using SMS.Application.DTOs.Create;
using SMS.Application.DTOs.Response;
using SMS.Application.DTOs.Update;

namespace SMS.Application.Services;
public sealed class TeacherService
{
    private readonly ITeacherRepository _teacherRepository;
    public TeacherService(ITeacherRepository teacherRepository)
    {
        _teacherRepository = teacherRepository;
    }
    public async Task<TeacherMstResponse> CreateAsync(CreateTeacherMstRequest request, CancellationToken cancellationToken = default)
    {
        var schoolExists = await _teacherRepository.SchoolExistsAsync(request.SchoolId, cancellationToken);
        if (!schoolExists)
        {
            throw new NotFoundException($"School with ID {request.SchoolId} was not found.", request.SchoolId);
        }
        var teacherCodeExists =
            await _teacherRepository.ExistsByTeacherCodeAsync(request.SchoolId, request.TeacherCode, cancellationToken: cancellationToken);

        if (teacherCodeExists)
        {
            throw new ConflictException($"Teacher code '{request.TeacherCode}' already exists for this school.");
        }
        var employeeCodeExists =
            await _teacherRepository.ExistsByEmployeeCodeAsync(request.SchoolId, request.EmployeeCode, cancellationToken: cancellationToken);

        if (employeeCodeExists)
        {
            throw new ConflictException($"Employee code '{request.EmployeeCode}' already exists for this school.");
        }
        var teacher = TeacherMst.Create(
            request.SchoolId,
            request.TeacherCode,
            request.EmployeeCode,
            request.FirstName,
            request.MiddleName,
            request.LastName,
            request.Gender,
            request.DateOfBirth,
            request.Email,
            request.MobileNumber,
            request.DateOfJoining,
            request.Qualification,
            request.Designation,
            createdBy: null,
            createdAt: DateTimeOffset.UtcNow);
        await _teacherRepository.AddAsync(teacher, cancellationToken);
        await _teacherRepository.SaveChangesAsync(cancellationToken);
        return MapToResponse(teacher);
    }
    public async Task<TeacherMstResponse> GetByIdAsync(long teacherId, CancellationToken cancellationToken = default)
    {
        var teacher =
            await _teacherRepository.GetByIdAsync(teacherId, cancellationToken);

        if (teacher is null)
        {
            throw new NotFoundException($"Teacher with ID {teacherId} was not found.", teacherId);
        }
        return MapToResponse(teacher);
    }
    public async Task<IReadOnlyList<TeacherMstResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var teachers =
            await _teacherRepository.GetAllAsync(cancellationToken);
        return teachers.Select(MapToResponse).ToList();
    }
    public async Task<TeacherMstResponse> UpdateAsync(long teacherId, UpdateTeacherMstRequest request, CancellationToken cancellationToken = default)
    {
        var teacher =
            await _teacherRepository.GetByIdAsync(teacherId, cancellationToken);

        if (teacher is null)
        {
            throw new NotFoundException($"Teacher with ID {teacherId} was not found.", teacherId);
        }
        teacher.UpdateDetails(
            request.FirstName,
            request.MiddleName,
            request.LastName,
            request.Gender,
            request.DateOfBirth,
            request.Email,
            request.MobileNumber,
            request.DateOfJoining,
            request.Qualification,
            request.Designation,
            updatedBy: null,
            updatedAt: DateTimeOffset.UtcNow);
        await _teacherRepository.SaveChangesAsync(cancellationToken);
        return MapToResponse(teacher);
    }
    public async Task ActivateAsync(long teacherId, CancellationToken cancellationToken = default)
    {
        var teacher =
            await _teacherRepository.GetByIdAsync(teacherId, cancellationToken);

        if (teacher is null)
        {
            throw new NotFoundException($"Teacher with ID {teacherId} was not found.", teacherId);
        }

        teacher.Activate(updatedBy: null, updatedAt: DateTimeOffset.UtcNow);
        await _teacherRepository.SaveChangesAsync(cancellationToken);
    }
    public async Task DeactivateAsync(long teacherId, CancellationToken cancellationToken = default)
    {
        var teacher =
            await _teacherRepository.GetByIdAsync(teacherId, cancellationToken);

        if (teacher is null)
        {
            throw new NotFoundException($"Teacher with ID {teacherId} was not found.", teacherId);
        }

        teacher.Deactivate(updatedBy: null, updatedAt: DateTimeOffset.UtcNow);
        await _teacherRepository.SaveChangesAsync(cancellationToken);
    }
    private static TeacherMstResponse MapToResponse(TeacherMst teacher)
    {
        return new TeacherMstResponse
        {
            TeacherId = teacher.TeacherId,
            SchoolId = teacher.SchoolId,
            TeacherCode = teacher.TeacherCode,
            EmployeeCode = teacher.EmployeeCode,
            FirstName = teacher.FirstName,
            MiddleName = teacher.MiddleName,
            LastName = teacher.LastName,
            Gender = teacher.Gender,
            DateOfBirth = teacher.DateOfBirth,
            Email = teacher.Email,
            MobileNumber = teacher.MobileNumber,
            DateOfJoining = teacher.DateOfJoining,
            Qualification = teacher.Qualification,
            Designation = teacher.Designation,
            IsActive = teacher.IsActive,
            CreatedAt = teacher.CreatedAt,
            CreatedBy = teacher.CreatedBy,
            UpdatedAt = teacher.UpdatedAt,
            UpdatedBy = teacher.UpdatedBy
        };
    }
}
