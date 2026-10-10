using SMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Application.Interfaces;
public interface IAttendanceTypeRepository
{
    Task AddAsync(
        AttendanceTypeMst attendanceType,
        CancellationToken cancellationToken = default);

    Task<AttendanceTypeMst?> GetByIdAsync(
        long attendanceTypeId,
        CancellationToken cancellationToken = default);

    Task<AttendanceTypeMst?> GetByCodeAsync(
        string attendanceTypeCode,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AttendanceTypeMst>> GetAllAsync(
        bool activeOnly = false,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByCodeAsync(
        string attendanceTypeCode,
        long? excludeAttendanceTypeId = null,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        AttendanceTypeMst attendanceType,
        CancellationToken cancellationToken = default);
}
