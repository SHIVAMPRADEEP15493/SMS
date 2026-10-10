using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Application.DTOs;
public sealed class UpdateSectionMstRequest
{
    public string SectionName { get; init; } = null!;
    public int DisplayOrder { get; init; }
}
