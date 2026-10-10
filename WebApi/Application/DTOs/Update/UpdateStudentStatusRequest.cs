using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Application.DTOs.Update;
public sealed class UpdateStudentStatusRequest
{
    public string Status { get; set; } = null!;
    public long? UpdatedBy { get; set; }
    //public long Version { get; set; }
}
