using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Application.Common.Exceptions;
public class NotFoundException : ApplicationExceptionBase
{
    public NotFoundException( string entityName, object entityId) : base($"{entityName} with id '{entityId}' was not found.")
    {
    }
}
