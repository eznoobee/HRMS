using HRMS.Application.Common.Models;
using HRMS.Domain.Enums;
using MediatR;

namespace HRMS.Application.Features.Auth.Commands.Register;

public record RegisterCommand(
    string FirstName,
    string FatherName,
    string GrandfatherName,
    string FamilyName,
    string Email,
    string Password,
    string? Phone,
    DateOnly DateOfBirth,
    DateOnly JoinDate,
    string? JobTitle,
    UserRole Role,
    Guid CompanyId,
    Guid DepartmentId
) : IRequest<Result<RegisterResponse>>;

public record RegisterResponse(Guid EmployeeId, string Email, string FullName);
