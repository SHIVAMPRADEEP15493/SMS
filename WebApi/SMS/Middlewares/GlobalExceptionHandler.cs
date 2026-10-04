using System.Text;
using SMS.Domain.Exceptions;
using SMS.Application.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace SMS.Api.Middlewares;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    /*
    private readonly ILogger<GlobalExceptionHandler> _logger;
    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync( HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError( exception, "An unhandled exception occurred.");

        var problemDetails = CreateProblemDetails(exception);
        httpContext.Response.StatusCode =
            problemDetails.Status ?? StatusCodes.Status500InternalServerError;

        await httpContext.Response.WriteAsJsonAsync( problemDetails, cancellationToken);
        return true;
    }

    private static ProblemDetails CreateProblemDetails(Exception exception)
    {
        return exception switch
        {
            NotFoundException =>
                new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Resource not found",
                    Detail = exception.Message
                },

            ConflictException =>
                new ProblemDetails
                {
                    Status = StatusCodes.Status409Conflict,
                    Title = "Conflict",
                    Detail = exception.Message
                },

            _ => new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "An unexpected error occurred.",
                    Detail = "An unexpected error occurred while processing the request."
                }
        };
    }
    */

    /*
    private readonly ILogger<GlobalExceptionHandler> _logger;
    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(
            exception,
            "An unhandled exception occurred.");

        var problemDetails = CreateProblemDetails(
            httpContext,
            exception);

        httpContext.Response.StatusCode =
            problemDetails.Status
            ?? StatusCodes.Status500InternalServerError;

        httpContext.Response.ContentType =
            "application/problem+json";

        await httpContext.Response.WriteAsJsonAsync(
            problemDetails,
            cancellationToken);

        return true;
    }

    private static ProblemDetails CreateProblemDetails( HttpContext httpContext, Exception exception)
    {
        return exception switch
        {
            ValidationException validationException =>
                CreateValidationProblemDetails(httpContext, validationException),

            NotFoundException => new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Resource not found",
                    Detail = exception.Message,
                    Instance = httpContext.Request.Path
                },

            ConflictException => new ProblemDetails
                {
                    Status = StatusCodes.Status409Conflict,
                    Title = "Conflict",
                    Detail = exception.Message,
                    Instance = httpContext.Request.Path
                },

            _ =>
                new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,

                    Title = "An unexpected error occurred.",

                    Detail =
                        "An unexpected error occurred while processing the request.",

                    Instance = httpContext.Request.Path
                }
        };
    }

    CreateValidationProblemDetails(HttpContext httpContext, ValidationException exception)
    {
        var errors = exception.Errors.ToDictionary(
            x => x.Key,
            x => x.Value,
            StringComparer.OrdinalIgnoreCase);

        var problemDetails = new ValidationProblemDetails(errors)
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Validation failed",
            Detail = "One or more validation errors occurred.",
            Instance = httpContext.Request.Path
        };

        problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;
        return problemDetails;
    }

    */
    private readonly ILogger<GlobalExceptionHandler> _logger;
    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Unhandled exception occurred. TraceId: {TraceId}", httpContext.TraceIdentifier);
        var problemDetails = CreateProblemDetails(httpContext, exception);
        httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
        httpContext.Response.ContentType = "application/problem+json";
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }

    private static ProblemDetails CreateProblemDetails(HttpContext httpContext, Exception exception)
    {
        return exception switch
        {
            ValidationException validationException =>
                CreateValidationProblemDetails(httpContext, validationException),

            NotFoundException =>
                CreateProblemDetails(httpContext, StatusCodes.Status404NotFound, "Resource not found", exception.Message),

            ConflictException =>
                CreateProblemDetails(httpContext, StatusCodes.Status409Conflict, "Conflict", exception.Message),

            DomainRuleViolationException domainException =>
                CreateProblemDetails(httpContext, StatusCodes.Status409Conflict,"Business rule violation", domainException.Message),

            _ =>
                CreateProblemDetails(
                    httpContext, 
                    StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred.", 
                    "An unexpected error occurred while processing the request.")
        };
    }
    private static ProblemDetails CreateProblemDetails(HttpContext httpContext, int statusCode, string title, string detail)
    {
        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = httpContext.Request.Path
        };
        problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;
        return problemDetails;
    }

    private static ValidationProblemDetails
        CreateValidationProblemDetails(HttpContext httpContext, ValidationException exception)
        {
            var errors = exception.Errors.ToDictionary(
                x => x.Key,
                x => x.Value,
                StringComparer.OrdinalIgnoreCase);

            var errorMessages = new StringBuilder();
            errorMessages.AppendLine("One or more validation errors occurred:");
            foreach (var kvp in errors)
            {
                errorMessages.AppendLine($"{kvp.Key}: {string.Join(", ", kvp.Value)}");
            }
            var problemDetails =
                new ValidationProblemDetails(errors)
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Validation failed",
                    Detail = errorMessages.ToString().Trim(),
                    Instance = httpContext.Request.Path
                };
            problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;
            return problemDetails;
        }
}
