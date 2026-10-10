using FluentValidation;
using SMS.Application.DTOs.Update;

namespace SMS.Application.Validators.UpdateValidators;

public sealed class UpdateStudentEnrollmentStatusRequestValidator
    : AbstractValidator<UpdateStudentEnrollmentStatusRequest>
{
    private static readonly string[] AllowedStatuses =
    [
        "Active",
        "Completed",
        "Transferred",
        "Withdrawn"
    ];

    public UpdateStudentEnrollmentStatusRequestValidator()
    {
        RuleFor(x => x.Status)
            .NotEmpty()
            .WithMessage("Enrollment status is required.")
            .Must(status =>
                !string.IsNullOrWhiteSpace(status) &&
                AllowedStatuses.Contains(
                    status.Trim(),
                    StringComparer.OrdinalIgnoreCase))
            .WithMessage(
                "Status must be Active, Completed, Transferred, or Withdrawn.");
    }
}