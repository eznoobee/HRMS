using HRMS.Application.Common.Models;
using MediatR;

namespace HRMS.Application.Features.Employees.Commands.DeleteEmployee;

public record DeleteEmployeeCommand(Guid EmployeeId) : IRequest<Result<DeleteEmployeeResponse>>;

public record DeleteEmployeeResponse(Guid EmployeeId);
