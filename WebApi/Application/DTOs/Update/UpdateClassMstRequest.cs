using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Application.DTOs.Update;
public sealed record UpdateClassMstRequest(
    string ClassName,
    int DisplayOrder);