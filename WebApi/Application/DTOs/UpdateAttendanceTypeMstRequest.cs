using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Application.DTOs;
public sealed record UpdateAttendanceTypeMstRequest(
    string AttendanceTypeCode,
    string AttendanceTypeName,
    long? UpdatedBy = null);