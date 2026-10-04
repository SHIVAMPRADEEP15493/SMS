using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Application.Common.Exceptions;
public sealed class ValidationException : ApplicationExceptionBase
{
    public IReadOnlyDictionary<string, string[]> Errors { get; }
    public ValidationException(IReadOnlyDictionary<string, string[]> errors) : base(BuildErrorMessage(errors))
    {
        Errors = errors;
    }

    private static string BuildErrorMessage(IReadOnlyDictionary<string, string[]> errors)
    {
        if (errors == null || errors.Count == 0)
        {
            return "One or more validation errors occurred.";
        }
        var errorMessages = new StringBuilder();
        errorMessages.AppendLine("One or more validation errors occurred:");
        foreach (var kvp in errors)
        {
            errorMessages.AppendLine($"{kvp.Key}: {string.Join(", ", kvp.Value)}");
        }
        return errorMessages.ToString().TrimEnd();
    }
}
