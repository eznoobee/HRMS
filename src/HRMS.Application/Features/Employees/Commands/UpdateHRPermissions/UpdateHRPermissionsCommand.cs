using HRMS.Application.Common.Models;
using HRMS.Domain.Enums;
using MediatR;

namespace HRMS.Application.Features.Employees.Commands.UpdateHRPermissions;

public record UpdateHRPermissionsCommand(Guid EmployeeId, IReadOnlyList<HRPermission> Permissions) : IRequest<Result>;
