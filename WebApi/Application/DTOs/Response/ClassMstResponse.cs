using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Application.DTOs.Response;
public sealed record ClassMstResponse(
    long ClassId,
    string ClassCode,
    string ClassName,
    int DisplayOrder,
    bool IsActive,
    DateTime CreatedAt,
    long? CreatedBy,
    DateTime? UpdatedAt,
    long? UpdatedBy);
