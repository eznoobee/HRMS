using FluentValidation;
using HRMS.Domain.ValueObjects;

namespace HRMS.Application.Features.Auth.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.FatherName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.GrandfatherName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.FamilyName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256);
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8)
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one digit.");
        RuleFor(x => x.Phone)
            .Must(PhoneNumber.IsValid)
            .WithMessage("Phone must be a valid number (e.g., 07XXXXXXXXX or +964XXXXXXXXX).")
            .When(x => x.Phone is not null);
        RuleFor(x => x.DepartmentId).NotEmpty();
        RuleFor(x => x.DateOfBirth).NotEmpty();
        RuleFor(x => x.JoinDate).NotEmpty();
    }
}
