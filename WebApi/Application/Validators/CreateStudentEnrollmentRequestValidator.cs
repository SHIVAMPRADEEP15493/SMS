using FluentValidation;
using SMS.Application.Features.StudentEnrollments.DTOs;

namespace SMS.Application.Validators;

public sealed class CreateStudentEnrollmentRequestValidator
    : AbstractValidator<CreateStudentEnrollmentRequest>
{
    public CreateStudentEnrollmentRequestValidator()
    {
        RuleFor(x => x.StudentId)
            .GreaterThan(0)
            .WithMessage("Student ID must be greater than zero.");

        RuleFor(x => x.SchoolId)
            .GreaterThan(0)
            .WithMessage("School ID must be greater than zero.");

        RuleFor(x => x.SessionId)
            .GreaterThan(0)
            .WithMessage("Session ID must be greater than zero.");

        RuleFor(x => x.ClassId)
            .GreaterThan(0)
            .WithMessage("Class ID must be greater than zero.");

        RuleFor(x => x.SectionId)
            .GreaterThan(0)
            .WithMessage("Section ID must be greater than zero.");

        RuleFor(x => x.AdmissionDate)
            .Must(date => date >= new DateOnly(1900, 1, 1))
            .WithMessage("Admission date must be on or after 1900-01-01.");

        RuleFor(x => x.RollNumber)
            .NotEmpty()
            .WithMessage("Roll number is required.")
            .Must(value => !string.IsNullOrWhiteSpace(value))
            .WithMessage("Roll number cannot contain only whitespace.")
            .MaximumLength(20)
            .WithMessage("Roll number cannot exceed 20 characters.");
    }
}
