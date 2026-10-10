using FluentValidation;
using SMS.Application.DTOs.Update;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Application.Validators.UpdateValidators;
public sealed class UpdateAttendanceTypeMstRequestValidator : AbstractValidator<UpdateAttendanceTypeMstRequest>
{
    public UpdateAttendanceTypeMstRequestValidator()
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

        RuleFor(x => x.UpdatedBy)
            .GreaterThan(0)
            .When(x => x.UpdatedBy.HasValue)
            .WithMessage("UpdatedBy must be greater than zero.");
    }
}
