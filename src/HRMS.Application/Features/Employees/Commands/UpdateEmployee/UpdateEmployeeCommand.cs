using HRMS.Application.Common.Models;
using HRMS.Domain.Enums;
using MediatR;

namespace HRMS.Application.Features.Employees.Commands.UpdateEmployee;

public record UpdateEmployeeCommand(
    Guid EmployeeId,
    string FirstName,
    string LastName,
    string? Phone,
    string? AvatarUrl,
    DateOnly DateOfBirth,
    DateOnly JoinDate,
    string? JobTitle,
    UserRole Role,
    Guid DepartmentId,
    bool IsActive
) : IRequest<Result<UpdateEmployeeResponse>>;

public record UpdateEmployeeResponse(Guid EmployeeId, string FullName);
