using FluentValidation;

namespace HRMS.Application.Features.Overtime.Commands.ApplyOvertime;

public class ApplyOvertimeCommandValidator : AbstractValidator<ApplyOvertimeCommand>
{
    public ApplyOvertimeCommandValidator()
    {
        RuleFor(x => x.StartTime).NotEmpty();
        RuleFor(x => x.EndTime).NotEmpty()
            .GreaterThan(x => x.StartTime)
            .WithMessage("End time must be after start time.");
        RuleFor(x => x.Reason).MaximumLength(1000);
    }
}
