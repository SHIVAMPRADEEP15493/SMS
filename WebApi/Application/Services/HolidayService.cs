using SMS.Application.Common.Exceptions;
using SMS.Application.DTOs;
using SMS.Application.Interfaces;
using SMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Application.Services;
public sealed class HolidayService
{
    private readonly IHolidayRepository _repository;
    public HolidayService(IHolidayRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<HolidayDtlResponse>> GetAllAsync(
        long? schoolId,
        long? sessionId,
        CancellationToken cancellationToken = default)
    {
        var holidays = await _repository.GetAllAsync(
            schoolId,
            sessionId,
            cancellationToken);

        return holidays.Select(MapToResponse).ToList();
    }

    public async Task<HolidayDtlResponse> GetByIdAsync(
        long holidayId,
        CancellationToken cancellationToken = default)
    {
        var holiday = await _repository.GetByIdAsync(
            holidayId,
            cancellationToken);

        if (holiday is null)
        {
            throw new NotFoundException(nameof(HolidayDtl), holidayId);
        }

        return MapToResponse(holiday);
    }

    public async Task<HolidayDtlResponse> CreateAsync(
        CreateHolidayDtlRequest request,
        CancellationToken cancellationToken = default)
    {
        // Validate description
        if (string.IsNullOrWhiteSpace(request.HolidayDescription))
        {
            throw new ValidationException(
                new Dictionary<string, string[]>
                {
                    ["HolidayDescription"] = new[] { "Holiday description is required." }
                });
        }

        await ValidateSchoolAndSessionAsync(
            request.SchoolId,
            request.SessionId,
            request.HolidayDate,
            cancellationToken);

        var duplicate = await _repository
            .ExistsForSchoolSessionDateAsync(
                request.SchoolId,
                request.SessionId,
                request.HolidayDate,
                cancellationToken: cancellationToken);

        if (duplicate)
        {
            throw new ConflictException(
                "A holiday already exists for this school, session, and date.");
        }

        var holiday = new HolidayDtl
        {
            SchoolId = request.SchoolId,
            SessionId = request.SessionId,
            HolidayDate = request.HolidayDate,
            HolidayDescription = request.HolidayDescription.Trim(),
            CreatedAt = DateTimeOffset.UtcNow
        };

        await _repository.AddAsync(holiday, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return MapToResponse(holiday);
    }

    public async Task<HolidayDtlResponse> UpdateAsync(
        long holidayId,
        UpdateHolidayDtlRequest request,
        CancellationToken cancellationToken = default)
    {
        // Validate description
        if (string.IsNullOrWhiteSpace(request.HolidayDescription))
        {
            throw new ValidationException(
                new Dictionary<string, string[]>
                {
                    ["HolidayDescription"] = new[] { "Holiday description is required." }
                });
        }

        var holiday = await _repository.GetByIdAsync(
            holidayId,
            cancellationToken);

        if (holiday is null)
        {
            throw new NotFoundException(nameof(HolidayDtl), holidayId);
        }

        await ValidateSchoolAndSessionAsync(
            request.SchoolId,
            request.SessionId,
            request.HolidayDate,
            cancellationToken);

        var duplicate = await _repository
            .ExistsForSchoolSessionDateAsync(
                request.SchoolId,
                request.SessionId,
                request.HolidayDate,
                excludeHolidayId: holidayId,
                cancellationToken: cancellationToken);

        if (duplicate)
        {
            throw new ConflictException(
                "A holiday already exists for this school, session, and date.");
        }

        holiday.SchoolId = request.SchoolId;
        holiday.SessionId = request.SessionId;
        holiday.HolidayDate = request.HolidayDate;
        holiday.HolidayDescription = request.HolidayDescription.Trim();
        holiday.UpdatedAt = DateTimeOffset.UtcNow;

        _repository.Update(holiday);
        await _repository.SaveChangesAsync(cancellationToken);

        return MapToResponse(holiday);
    }

    public async Task DeleteAsync(
        long holidayId,
        CancellationToken cancellationToken = default)
    {
        var holiday = await _repository.GetByIdAsync(
            holidayId,
            cancellationToken);

        if (holiday is null)
        {
            throw new NotFoundException(nameof(HolidayDtl), holidayId);
        }

        _repository.Delete(holiday);
        await _repository.SaveChangesAsync(cancellationToken);
    }

    private async Task ValidateSchoolAndSessionAsync(
        long schoolId,
        long sessionId,
        DateOnly holidayDate,
        CancellationToken cancellationToken)
    {
        var schoolExists = await _repository.SchoolExistsAsync(
            schoolId,
            cancellationToken);

        if (!schoolExists)
        {
            throw new NotFoundException(nameof(SchoolMst), schoolId);
        }

        var session = await _repository.GetSessionByIdAsync(
            sessionId,
            cancellationToken);

        if (session is null)
        {
            throw new NotFoundException(nameof(SessionMst), sessionId);
        }

        if (holidayDate < session.StartDate ||
            holidayDate > session.EndDate)
        {
            throw new ValidationException(          
                new Dictionary<string, string[]>
                {
                    ["HolidayDate"] = new[] { "Holiday date must fall within the selected academic session." }
                });
        }
    }

    private static HolidayDtlResponse MapToResponse(HolidayDtl holiday)
    {
        return new HolidayDtlResponse
        {
            HolidayId = holiday.HolidayId,
            SchoolId = holiday.SchoolId,
            SessionId = holiday.SessionId,
            HolidayDate = holiday.HolidayDate,
            HolidayDescription = holiday.HolidayDescription,
            CreatedAt = holiday.CreatedAt,
            CreatedBy = holiday.CreatedBy,
            UpdatedAt = holiday.UpdatedAt,
            UpdatedBy = holiday.UpdatedBy
        };
    }
}
