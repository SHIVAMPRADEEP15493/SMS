using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using SMS.Application.DTOs.Update;

namespace SMS.Application.Validators.UpdateValidators;
public sealed class UpdateSessionMstRequestValidator : AbstractValidator<UpdateSessionMstRequest>
{
    public UpdateSessionMstRequestValidator()
    {
        RuleFor(x => x.SessionName)
            .NotEmpty()
            .WithMessage("Session name is required.")
            .MaximumLength(100)
            .WithMessage("Session name cannot exceed 100 characters.");

        RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithMessage("Start date is required.");

        RuleFor(x => x.EndDate)
            .NotEmpty()
            .WithMessage("End date is required.");

        RuleFor(x => x)
            .Must(x => x.StartDate < x.EndDate)
            .WithMessage("Start date must be earlier than end date.");
    }
}
