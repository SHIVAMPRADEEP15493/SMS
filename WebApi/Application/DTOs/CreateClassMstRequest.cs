using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Application.DTOs;
public sealed record CreateClassMstRequest(
    string ClassCode,
    string ClassName,
    int DisplayOrder);

