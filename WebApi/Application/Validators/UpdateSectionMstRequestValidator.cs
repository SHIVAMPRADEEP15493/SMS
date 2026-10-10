using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using SMS.Application.DTOs;

namespace SMS.Application.Validators;
public sealed class UpdateSectionMstRequestValidator : AbstractValidator<UpdateSectionMstRequest>
{
    public UpdateSectionMstRequestValidator()
    {
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
