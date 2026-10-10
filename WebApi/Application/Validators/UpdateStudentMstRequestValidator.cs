using FluentValidation;
using SMS.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Application.Validators;
public sealed class UpdateStudentMstRequestValidator : AbstractValidator<UpdateStudentMstRequest>
{
    public UpdateStudentMstRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.MiddleName)
            .MaximumLength(100);

        RuleFor(x => x.LastName)
            .MaximumLength(100);

        RuleFor(x => x.DateOfBirth)
            .NotEmpty()
            .LessThanOrEqualTo(
                DateOnly.FromDateTime(DateTime.UtcNow));

        RuleFor(x => x.Gender)
            .NotEmpty()
            .MaximumLength(20)
            .Must(BeValidGender)
            .WithMessage(
                "Gender must be Male, Female, or Other.");

        RuleFor(x => x.MobileNumber)
            .MaximumLength(20)
            .Matches(@"^[0-9+\-\s()]*$")
            .When(x => !string.IsNullOrWhiteSpace(x.MobileNumber))
            .WithMessage("Mobile number format is invalid.");

        RuleFor(x => x.Email)
            .MaximumLength(150)
            .EmailAddress()
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.Address)
            .MaximumLength(500);

        RuleFor(x => x.FatherName)
            .MaximumLength(200);

        RuleFor(x => x.MotherName)
            .MaximumLength(200);

        RuleFor(x => x.GuardianName)
            .MaximumLength(200);

        RuleFor(x => x.GuardianMobileNumber)
            .MaximumLength(20)
            .Matches(@"^[0-9+\-\s()]*$")
            .When(x => !string.IsNullOrWhiteSpace(x.GuardianMobileNumber))
            .WithMessage("Guardian Mobile number format is invalid.");

        RuleFor(x => x.UpdatedBy)
            .GreaterThan(0)
            .When(x => x.UpdatedBy.HasValue);
    }

    private static bool BeValidGender(string gender)
    {
        return gender.Equals("Male", StringComparison.OrdinalIgnoreCase)
            || gender.Equals("Female", StringComparison.OrdinalIgnoreCase)
            || gender.Equals("Other", StringComparison.OrdinalIgnoreCase);
    }
}
