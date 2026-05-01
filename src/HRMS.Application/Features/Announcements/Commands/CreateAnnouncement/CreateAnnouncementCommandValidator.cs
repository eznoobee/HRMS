using FluentValidation;
using HRMS.Domain.Enums;

namespace HRMS.Application.Features.Announcements.Commands.CreateAnnouncement;

public class CreateAnnouncementCommandValidator : AbstractValidator<CreateAnnouncementCommand>
{
    public CreateAnnouncementCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Body).NotEmpty().MaximumLength(5000);
        RuleFor(x => x.DepartmentId)
            .NotEmpty()
            .When(x => x.Scope == AnnouncementScope.Department)
            .WithMessage("Department is required for department-scoped announcements.");
    }
}
