using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Application.Common.Exceptions;
public abstract class ApplicationExceptionBase : Exception
{
    protected ApplicationExceptionBase(string message) : base(message)
    {
    }

    protected ApplicationExceptionBase( string message, Exception innerException) : base(message, innerException)
    {
    }
}
