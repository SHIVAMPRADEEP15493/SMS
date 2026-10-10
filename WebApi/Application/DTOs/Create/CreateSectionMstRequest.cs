using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Application.DTOs.Create;
public sealed class CreateSectionMstRequest
{
    public string SectionCode { get; init; } = null!;
    public string SectionName { get; init; } = null!;
    public int DisplayOrder { get; init; }
}
