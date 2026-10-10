using FluentValidation;
using SMS.Application.DTOs.Update;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Application.Validators.UpdateValidators;
public sealed class UpdateHolidayDtlRequestValidator : AbstractValidator<UpdateHolidayDtlRequest>
{
    public UpdateHolidayDtlRequestValidator()
    {
        RuleFor(x => x.SchoolId)
            .GreaterThan(0)
            .WithMessage("School ID must be greater than zero.");

        RuleFor(x => x.SessionId)
            .GreaterThan(0)
            .WithMessage("Session ID must be greater than zero.");

        RuleFor(x => x.HolidayDate)
            .NotEmpty()
            .WithMessage("Holiday date is required.");

        RuleFor(x => x.HolidayDescription)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Holiday description is required.")
            .Must(description => !string.IsNullOrWhiteSpace(description))
            .WithMessage("Holiday description cannot contain only whitespace.")
            .MaximumLength(200)
            .WithMessage("Holiday description cannot exceed 200 characters.");
    }
}
