using FluentValidation;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using ApplicationValidationException = SMS.Application.Common.Exceptions.ValidationException;

namespace SMS.Api.Filters;

public sealed class ValidationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        var validationErrors =
            new Dictionary<string, List<string>>(
                StringComparer.OrdinalIgnoreCase);

        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument is null)
            {
                continue;
            }

            var argumentType = argument.GetType();

            var validatorType = typeof(IValidator<>)
                .MakeGenericType(argumentType);

            var validator =
                context.HttpContext.RequestServices
                    .GetService(validatorType);

            if (validator is null)
            {
                continue;
            }

            var validationContextType =
                typeof(ValidationContext<>)
                    .MakeGenericType(argumentType);

            var validationContext =
                Activator.CreateInstance(
                    validationContextType,
                    argument);

            if (validationContext is null)
            {
                continue;
            }

            var validateAsyncMethod =
                validatorType.GetMethod(
                    //nameof(IValidator<object>.ValidateAsync));
                    nameof(IValidator<object>.ValidateAsync));

            if (validateAsyncMethod is null)
            {
                continue;
            }

            //var task =
            //    (Task)validateAsyncMethod.Invoke(
            //        validator,
            //        new[]
            //        {
            //            validationContext,
            //            context.HttpContext.RequestAborted
            //        })!;
            var task =
                (Task)validateAsyncMethod.Invoke(
                    validator,
                    new[]
                    {
                        argument,  // Use the original argument instead of validationContext
                        context.HttpContext.RequestAborted
                    })!;

            await task.ConfigureAwait(false);

            var resultProperty =
                task.GetType().GetProperty("Result");

            var validationResult =
                resultProperty?.GetValue(task)
                as FluentValidation.Results.ValidationResult;

            if (validationResult is null ||
                validationResult.IsValid)
            {
                continue;
            }

            foreach (var error in validationResult.Errors)
            {
                var propertyName =
                    string.IsNullOrWhiteSpace(error.PropertyName)
                        ? argumentType.Name
                        : error.PropertyName;

                if (!validationErrors.TryGetValue(
                        propertyName,
                        out var errors))
                {
                    errors = new List<string>();
                    validationErrors[propertyName] = errors;
                }

                errors.Add(error.ErrorMessage);
            }
        }

        if (validationErrors.Count > 0)
        {
            IReadOnlyDictionary<string, string[]> errors = validationErrors.ToDictionary(
                                                            x => x.Key,
                                                            x => x.Value.Distinct().ToArray(),
                                                            StringComparer.OrdinalIgnoreCase);
            throw new ApplicationValidationException(errors);
        }
        await next();
    }
}


//using FluentValidation;
//using Microsoft.AspNetCore.Mvc.Filters;
//using ApplicationValidationException =
//    SMS.Application.Common.Exceptions.ValidationException;

//namespace SMS.Api.Filters;

//public sealed class ValidationFilter : IAsyncActionFilter
//{
//    public async Task OnActionExecutionAsync(
//        ActionExecutingContext context,
//        ActionExecutionDelegate next)
//    {
//        var validationErrors =
//            new Dictionary<string, List<string>>(
//                StringComparer.OrdinalIgnoreCase);

//        foreach (var argument in context.ActionArguments.Values)
//        {
//            if (argument is null)
//            {
//                continue;
//            }

//            var argumentType = argument.GetType();

//            var validatorType = typeof(IValidator<>)
//                .MakeGenericType(argumentType);

//            var validator =
//                context.HttpContext.RequestServices
//                    .GetService(validatorType);

//            if (validator is null)
//            {
//                continue;
//            }

//            var validationContextType =
//                typeof(ValidationContext<>)
//                    .MakeGenericType(argumentType);

//            var validationContext =
//                Activator.CreateInstance(
//                    validationContextType,
//                    argument);

//            if (validationContext is null)
//            {
//                continue;
//            }

//            var validateAsyncMethod =
//                validatorType.GetMethod(
//                    nameof(IValidator<object>.ValidateAsync),
//                    new[]
//                    {
//                        validationContextType,
//                        typeof(CancellationToken)
//                    });

//            if (validateAsyncMethod is null)
//            {
//                continue;
//            }

//            var task =
//                (Task)validateAsyncMethod.Invoke(
//                    validator,
//                    new[]
//                    {
//                        validationContext,
//                        context.HttpContext.RequestAborted
//                    })!;

//            await task.ConfigureAwait(false);

//            var resultProperty =
//                task.GetType().GetProperty("Result");

//            var validationResult =
//                resultProperty?.GetValue(task)
//                as FluentValidation.Results.ValidationResult;

//            if (validationResult is null ||
//                validationResult.IsValid)
//            {
//                continue;
//            }

//            foreach (var error in validationResult.Errors)
//            {
//                var propertyName =
//                    string.IsNullOrWhiteSpace(error.PropertyName)
//                        ? argumentType.Name
//                        : error.PropertyName;

//                if (!validationErrors.TryGetValue(
//                        propertyName,
//                        out var errors))
//                {
//                    errors = new List<string>();
//                    validationErrors[propertyName] = errors;
//                }

//                errors.Add(error.ErrorMessage);
//            }
//        }

//        if (validationErrors.Count > 0)
//        {
//            IReadOnlyDictionary<string, string[]> errors =
//                validationErrors.ToDictionary(
//                    x => x.Key,
//                    x => x.Value.Distinct().ToArray(),
//                    StringComparer.OrdinalIgnoreCase);

//            throw new ApplicationValidationException(errors);
//        }

//        await next();
//    }
//}
