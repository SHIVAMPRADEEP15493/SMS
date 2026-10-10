using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Application.DTOs;
public sealed class UpdateStudentStatusRequest
{
    public string Status { get; set; } = null!;
    public long? UpdatedBy { get; set; }
    //public long Version { get; set; }
}
