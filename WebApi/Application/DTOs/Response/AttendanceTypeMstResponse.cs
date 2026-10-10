using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Application.DTOs.Response;
public sealed record AttendanceTypeMstResponse
(
    long AttendanceTypeId,
    string AttendanceTypeCode,
    string AttendanceTypeName,
    bool IsActive,
    DateTimeOffset CreatedAt,
    long? CreatedBy,
    DateTimeOffset? UpdatedAt,
    long? UpdatedBy
);
