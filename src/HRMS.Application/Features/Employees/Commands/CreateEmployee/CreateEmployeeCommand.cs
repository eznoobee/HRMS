using HRMS.Application.Common.Models;
using HRMS.Domain.Enums;
using MediatR;

namespace HRMS.Application.Features.Employees.Commands.CreateEmployee;

public record CreateEmployeeCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string? Phone,
    DateOnly DateOfBirth,
    DateOnly JoinDate,
    string? JobTitle,
    UserRole Role,
    Guid DepartmentId
) : IRequest<Result<CreateEmployeeResponse>>;

public record CreateEmployeeResponse(Guid EmployeeId, string Email, string FullName);
