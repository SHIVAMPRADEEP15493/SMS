using SMS.Application.Common.Exceptions;
using SMS.Application.DTOs.Create;
using SMS.Application.DTOs.Response;
using SMS.Application.DTOs.Update;
using SMS.Application.Interfaces;
using SMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Application.Services;
public sealed class SchoolService
{
    private readonly ISchoolRepository _repository;
    public SchoolService(ISchoolRepository repository)
    {
        _repository = repository;
    }
    public async Task<SchoolMstDto> CreateAsync(CreateSchoolMstRequest request, CancellationToken cancellationToken)
    {
        var exists = await _repository.ExistsByCodeAsync(request.SchoolCode, cancellationToken);

        if (exists)
        {
            throw new ConflictException($"School with code '{request.SchoolCode}' already exists.");
        }

        var school = new SchoolMst(
                    request.SchoolCode,
                    request.SchoolName,
                    request.SchoolAddress,
                    request.City,
                    request.State,
                    request.Country,
                    request.PinCode,
                    request.PhoneNumber,
                    request.Email);

        await _repository.AddAsync(school, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return Map(school);
    }

    public async Task<IReadOnlyList<SchoolMstDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var schools = await _repository.GetAllAsync(cancellationToken);
        return schools.Select(Map).ToList();
    }

    public async Task<SchoolMstDto?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        var school = await _repository.GetByIdAsync(id, cancellationToken);
        if (school is null)
        {
            throw new NotFoundException(nameof(SchoolMst), id);
        }
        return Map(school);
    }

    private static SchoolMstDto Map(SchoolMst school)
    {
        return new SchoolMstDto
        {
            Id = school.Id,
            SchoolCode = school.SchoolCode,
            SchoolName = school.SchoolName,
            SchoolAddress = school.SchoolAddress,
            City = school.City,
            State = school.State,
            Country = school.Country,
            PinCode = school.PinCode,
            PhoneNumber = school.PhoneNumber,
            Email = school.Email,
            IsActive = school.IsActive
        };
    }
    public async Task<SchoolMstDto?> UpdateAsync(long id, UpdateSchoolMstRequest request, CancellationToken cancellationToken)
    {
        var school = await _repository.GetByIdAsync(id, cancellationToken);
        if (school is null)
        {
            throw new NotFoundException(nameof(SchoolMst), id);
        }
        school.Update(
            request.SchoolName,
            request.SchoolAddress,
            request.City,
            request.State,
            request.Country,
            request.PinCode,
            request.PhoneNumber,
            request.Email);

        _repository.Update(school);
        await _repository.SaveChangesAsync(cancellationToken);
        return Map(school);
    }
    public async Task<SchoolMstDto?> DeactivateAsync(long id, CancellationToken cancellationToken)
    {
        var school = await _repository.GetByIdAsync(id, cancellationToken);
        if (school is null)
        {
            throw new NotFoundException(nameof(SchoolMst), id);
        }
        school.Deactivate();
        _repository.Update(school);
        await _repository.SaveChangesAsync(cancellationToken);
        return Map(school);
    }
    public async Task<SchoolMstDto?> ActivateAsync(long id, CancellationToken cancellationToken)
    {
        var school = await _repository.GetByIdAsync(id, cancellationToken);
        if (school is null)
        {
            throw new NotFoundException(nameof(SchoolMst), id);
        }
        school.Activate();
        _repository.Update(school);
        await _repository.SaveChangesAsync(cancellationToken);
        return Map(school);
    }
}
