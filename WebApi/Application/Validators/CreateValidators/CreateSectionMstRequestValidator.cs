using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using SMS.Application.DTOs.Create;

namespace SMS.Application.Validators.CreateValidators;
public sealed class CreateSectionMstRequestValidator : AbstractValidator<CreateSectionMstRequest>
{
    public CreateSectionMstRequestValidator()
    {
        RuleFor(x => x.SectionCode)
            .NotEmpty()
            .WithMessage("Section code is required.")
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage("Section code cannot contain only whitespace.")
            .MaximumLength(20)
            .WithMessage("Section code must not exceed 20 characters.");

        RuleFor(x => x.SectionName)
            .NotEmpty()
            .WithMessage("Section name is required.")
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage("Section name cannot contain only whitespace.")
            .MaximumLength(100)
            .WithMessage("Section name must not exceed 100 characters.");

        RuleFor(x => x.DisplayOrder)
            .GreaterThan(0)
            .WithMessage("Display order must be greater than 0.");
    }
}
