using FluentValidation;
using HRMS.Domain.ValueObjects;

namespace HRMS.Application.Features.Employees.Commands.CreateEmployee;

public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
{
    public CreateEmployeeCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.FatherName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.GrandfatherName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.FamilyName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256);
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8).MaximumLength(128);
        RuleFor(x => x.Phone)
            .Must(PhoneNumber.IsValid)
            .WithMessage("Phone must be a valid number (e.g., 07XXXXXXXXX or +964XXXXXXXXX).")
            .When(x => x.Phone is not null);
        RuleFor(x => x.DateOfBirth).NotEmpty();
        RuleFor(x => x.JoinDate).NotEmpty();
        RuleFor(x => x.JobTitle).MaximumLength(200).When(x => x.JobTitle is not null);
        RuleFor(x => x.DepartmentId).NotEmpty();
    }
}
