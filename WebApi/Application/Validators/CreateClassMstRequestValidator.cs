using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using SMS.Application.DTOs;

namespace SMS.Application.Validators;
public sealed class CreateClassMstRequestValidator : AbstractValidator<CreateClassMstRequest>
{
    public CreateClassMstRequestValidator()
    {
        RuleFor(x => x.ClassCode)
            .NotEmpty()
            .WithMessage("Class code is required.")
            .MaximumLength(20)
            .WithMessage("Class code must not exceed 20 characters.");

        RuleFor(x => x.ClassName)
            .NotEmpty()
            .WithMessage("Class name is required.")
            .MaximumLength(100)
            .WithMessage("Class name must not exceed 100 characters.");

        RuleFor(x => x.DisplayOrder)
            .GreaterThan(0)
            .WithMessage("Display order must be greater than 0.");
    }
}
