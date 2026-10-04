using FluentValidation;
using SMS.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Application.Validators;
public class UpdateSchoolMstRequestValidator : AbstractValidator<UpdateSchoolMstRequest>
{
    public UpdateSchoolMstRequestValidator()
    {
        RuleFor(x => x.SchoolName)
            .NotEmpty()
            .WithMessage("School name is required.")
            .MaximumLength(200)
            .WithMessage("School name cannot exceed 200 characters.");

        RuleFor(x => x.SchoolAddress)
            .MaximumLength(500)
            .WithMessage("Address cannot exceed 500 characters.");

        RuleFor(x => x.City)
            .MaximumLength(100)
            .WithMessage("City name cannot exceed 100 characters.");

        RuleFor(x => x.State)
            .MaximumLength(100)
            .WithMessage("State name cannot exceed 100 characters.");

        RuleFor(x => x.Country)
            .MaximumLength(100)
            .WithMessage("Country name cannot exceed 100 characters.");

        RuleFor(x => x.PinCode)
            .MaximumLength(20)
            .WithMessage("Pin code cannot exceed 20 characters.");

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(30)
            .WithMessage("Phone number cannot exceed 30 characters.");

        RuleFor(x => x.Email)
            .MaximumLength(255)
            .WithMessage("Email cannot exceed 255 characters.")
            .EmailAddress()
            .When(x => !string.IsNullOrWhiteSpace(x.Email))
            .WithMessage("Email address is not valid.");
    }
}
