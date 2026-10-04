using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Domain.Exceptions;
public sealed class DomainRuleViolationException : Exception
{
    public DomainRuleViolationException(string message) : base(message)
    {
    }
}
