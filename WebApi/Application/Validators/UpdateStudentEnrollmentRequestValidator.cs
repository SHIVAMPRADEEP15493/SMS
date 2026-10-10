using FluentValidation;
using SMS.Application.Features.StudentEnrollments.DTOs;

namespace SMS.Application.Validators;

public sealed class UpdateStudentEnrollmentRequestValidator
    : AbstractValidator<UpdateStudentEnrollmentRequest>
{
    public UpdateStudentEnrollmentRequestValidator()
    {
        RuleFor(x => x.SchoolId)
            .GreaterThan(0)
            .WithMessage("School ID must be greater than zero.");

        RuleFor(x => x.ClassId)
            .GreaterThan(0)
            .WithMessage("Class ID must be greater than zero.");

        RuleFor(x => x.SectionId)
            .GreaterThan(0)
            .WithMessage("Section ID must be greater than zero.");

        RuleFor(x => x.RollNumber)
            .NotEmpty()
            .WithMessage("Roll number is required.")
            .Must(value => !string.IsNullOrWhiteSpace(value))
            .WithMessage("Roll number cannot contain only whitespace.")
            .MaximumLength(20)
            .WithMessage("Roll number cannot exceed 20 characters.");
    }
}
