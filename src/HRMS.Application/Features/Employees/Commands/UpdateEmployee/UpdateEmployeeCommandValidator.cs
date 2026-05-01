using FluentValidation;

namespace HRMS.Application.Features.Employees.Commands.UpdateEmployee;

public class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
{
    public UpdateEmployeeCommandValidator()
    {
        RuleFor(x => x.EmployeeId).NotEmpty();
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Phone).MaximumLength(30).When(x => x.Phone is not null);
        RuleFor(x => x.AvatarUrl).MaximumLength(500).When(x => x.AvatarUrl is not null);
        RuleFor(x => x.DateOfBirth).NotEmpty();
        RuleFor(x => x.JoinDate).NotEmpty();
        RuleFor(x => x.JobTitle).MaximumLength(200).When(x => x.JobTitle is not null);
        RuleFor(x => x.DepartmentId).NotEmpty();
    }
}
