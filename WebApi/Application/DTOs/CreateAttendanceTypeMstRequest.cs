using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Application.DTOs;
public sealed record CreateAttendanceTypeMstRequest(
    string AttendanceTypeCode,
    string AttendanceTypeName,
    long? CreatedBy = null);
