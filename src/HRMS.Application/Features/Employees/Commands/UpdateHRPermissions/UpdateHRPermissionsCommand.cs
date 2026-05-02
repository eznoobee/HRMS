using HRMS.Application.Common.Models;
using HRMS.Domain.Enums;
using MediatR;

namespace HRMS.Application.Features.Employees.Commands.UpdateHRPermissions;

public record UpdateHRPermissionsCommand(Guid EmployeeId, HRPermission Permissions) : IRequest<Result>;
