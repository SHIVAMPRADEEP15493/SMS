using SMS.Application.Interfaces;
using SMS.Domain.Entities;
using SMS.Application.Common.Exceptions;
using SMS.Application.DTOs.Create;
using SMS.Application.DTOs.Response;
using SMS.Application.DTOs.Update;

namespace SMS.Application.Services;
public sealed class AttendanceTypeService
{
    private readonly IAttendanceTypeRepository _repository;

    public AttendanceTypeService(
        IAttendanceTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<AttendanceTypeMstResponse> CreateAsync(
        CreateAttendanceTypeMstRequest request,
        CancellationToken cancellationToken = default)
    {
        var code = NormalizeCode(request.AttendanceTypeCode);

        var codeExists = await _repository.ExistsByCodeAsync(
            code,
            cancellationToken: cancellationToken);

        if (codeExists)
        {
            throw new ConflictException(
                $"Attendance type code '{code}' already exists.");
        }

        var attendanceType = new AttendanceTypeMst(
            code,
            request.AttendanceTypeName,
            request.CreatedBy);

        await _repository.AddAsync(
            attendanceType,
            cancellationToken);

        return MapToResponse(attendanceType);
    }

    public async Task<AttendanceTypeMstResponse> GetByIdAsync(
        long attendanceTypeId,
        CancellationToken cancellationToken = default)
    {
        var attendanceType = await GetEntityOrThrowAsync(
            attendanceTypeId,
            cancellationToken);

        return MapToResponse(attendanceType);
    }

    public async Task<IReadOnlyList<AttendanceTypeMstResponse>> GetAllAsync(
        bool activeOnly = false,
        CancellationToken cancellationToken = default)
    {
        var attendanceTypes = await _repository.GetAllAsync(
            activeOnly,
            cancellationToken);

        return attendanceTypes
            .Select(MapToResponse)
            .ToArray();
    }

    public async Task<AttendanceTypeMstResponse> UpdateAsync(
        long attendanceTypeId,
        UpdateAttendanceTypeMstRequest request,
        CancellationToken cancellationToken = default)
    {
        var attendanceType = await GetEntityOrThrowAsync(
            attendanceTypeId,
            cancellationToken);

        var code = NormalizeCode(request.AttendanceTypeCode);

        var codeExists = await _repository.ExistsByCodeAsync(
            code,
            excludeAttendanceTypeId: attendanceTypeId,
            cancellationToken: cancellationToken);

        if (codeExists)
        {
            throw new ConflictException(
                $"Attendance type code '{code}' already exists.");
        }

        attendanceType.Update(
            code,
            request.AttendanceTypeName,
            request.UpdatedBy);

        await _repository.UpdateAsync(
            attendanceType,
            cancellationToken);

        return MapToResponse(attendanceType);
    }

    public async Task<AttendanceTypeMstResponse> ActivateAsync(
        long attendanceTypeId,
        long? updatedBy = null,
        CancellationToken cancellationToken = default)
    {
        var attendanceType = await GetEntityOrThrowAsync(
            attendanceTypeId,
            cancellationToken);

        attendanceType.Activate(updatedBy);

        await _repository.UpdateAsync(
            attendanceType,
            cancellationToken);

        return MapToResponse(attendanceType);
    }

    public async Task<AttendanceTypeMstResponse> DeactivateAsync(
        long attendanceTypeId,
        long? updatedBy = null,
        CancellationToken cancellationToken = default)
    {
        var attendanceType = await GetEntityOrThrowAsync(
            attendanceTypeId,
            cancellationToken);

        attendanceType.Deactivate(updatedBy);

        await _repository.UpdateAsync(
            attendanceType,
            cancellationToken);

        return MapToResponse(attendanceType);
    }

    private async Task<AttendanceTypeMst> GetEntityOrThrowAsync(
        long attendanceTypeId,
        CancellationToken cancellationToken)
    {
        if (attendanceTypeId <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(attendanceTypeId),
                "Attendance type ID must be greater than zero.");
        }

        var attendanceType = await _repository.GetByIdAsync(
            attendanceTypeId,
            cancellationToken);

        if (attendanceType is null)
        {
            throw new NotFoundException(
                $"Attendance type with ID '{attendanceTypeId}' was not found.", attendanceTypeId);
        }

        return attendanceType;
    }

    private static string NormalizeCode(string code)
    {
        return code.Trim().ToUpperInvariant();
    }

    private static AttendanceTypeMstResponse MapToResponse(
        AttendanceTypeMst entity)
    {
        return new AttendanceTypeMstResponse(
            entity.AttendanceTypeId,
            entity.AttendanceTypeCode,
            entity.AttendanceTypeName,
            entity.IsActive,
            new DateTimeOffset(
                DateTime.SpecifyKind(entity.CreatedAt, DateTimeKind.Utc)),
            entity.CreatedBy,
            entity.UpdatedAt.HasValue
                ? new DateTimeOffset(
                    DateTime.SpecifyKind(
                        entity.UpdatedAt.Value,
                        DateTimeKind.Utc))
                : null,
            entity.UpdatedBy);
    }
}
