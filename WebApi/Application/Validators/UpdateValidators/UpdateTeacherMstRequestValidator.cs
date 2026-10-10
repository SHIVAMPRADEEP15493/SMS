using FluentValidation;
using SMS.Application.DTOs.Update;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Application.Validators.UpdateValidators;
public sealed class UpdateTeacherMstRequestValidator : AbstractValidator<UpdateTeacherMstRequest>
{
    public UpdateTeacherMstRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name is required.")
            .MaximumLength(50)
            .WithMessage("First name cannot exceed 50 characters.");

        RuleFor(x => x.MiddleName)
            .MaximumLength(50)
            .WithMessage("Middle name cannot exceed 50 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.MiddleName));

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name is required.")
            .MaximumLength(50)
            .WithMessage("Last name cannot exceed 50 characters.");

        RuleFor(x => x.Gender)
            .IsInEnum()
            .WithMessage("Invalid gender.");

        RuleFor(x => x.DateOfBirth)
            .Must(dateOfBirth =>
                !dateOfBirth.HasValue ||
                dateOfBirth.Value < DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("Date of birth must be in the past.");

        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Invalid email address.")
            .MaximumLength(150)
            .WithMessage("Email cannot exceed 150 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.MobileNumber)
            .Matches(@"^[0-9]{10}$")
            .WithMessage("Mobile number must contain exactly 10 digits.")
            .When(x => !string.IsNullOrWhiteSpace(x.MobileNumber));

        RuleFor(x => x.DateOfJoining)
            .LessThanOrEqualTo(
                DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("Date of joining cannot be in the future.");

        RuleFor(x => x.DateOfBirth)
            .LessThan(x => x.DateOfJoining)
            .When(x => x.DateOfBirth.HasValue)
            .WithMessage("Date of birth must be before date of joining.");

        RuleFor(x => x.Qualification)
            .MaximumLength(200)
            .WithMessage("Qualification cannot exceed 200 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Qualification));

        RuleFor(x => x.Designation)
            .MaximumLength(100)
            .WithMessage("Designation cannot exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Designation));
    }
}
