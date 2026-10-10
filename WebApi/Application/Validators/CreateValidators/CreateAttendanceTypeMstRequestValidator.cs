using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Text;
using SMS.Application.DTOs.Create;

namespace SMS.Application.Validators.CreateValidators;

public sealed class CreateAttendanceTypeMstRequestValidator : AbstractValidator<CreateAttendanceTypeMstRequest>
{
    public CreateAttendanceTypeMstRequestValidator()
    {
        RuleFor(x => x.AttendanceTypeCode)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Attendance type code is required.")
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage("Attendance type code cannot be blank.")
            .Must(x => x.Trim().Length <= 10)
            .WithMessage("Attendance type code cannot exceed 10 characters.")
            .Must(x => x.Trim().Length > 0)
            .WithMessage("Attendance type code cannot be blank.")
            .Must(x => x.Trim().All(char.IsLetterOrDigit))
            .WithMessage("Attendance type code must contain only letters and digits.");

        RuleFor(x => x.AttendanceTypeName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Attendance type name is required.")
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage("Attendance type name cannot be blank.")
            .Must(x => x.Trim().Length <= 50)
            .WithMessage("Attendance type name cannot exceed 50 characters.");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .When(x => x.CreatedBy.HasValue)
            .WithMessage("CreatedBy must be greater than zero.");
    }
}
