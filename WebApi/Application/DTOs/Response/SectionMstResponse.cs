using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Application.DTOs.Response;
public sealed class SectionMstResponse
{
    public long SectionId { get; init; }
    public string SectionCode { get; init; } = null!;
    public string SectionName { get; init; } = null!;
    public int DisplayOrder { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; init; }
    public long? CreatedBy { get; init; }
    public DateTime? UpdatedAt { get; init; }
    public long? UpdatedBy { get; init; }
}
