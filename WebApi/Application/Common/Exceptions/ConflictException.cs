using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Application.Common.Exceptions;
public class ConflictException : ApplicationExceptionBase
{
    public ConflictException(string message) : base(message)
    {
    }
}
