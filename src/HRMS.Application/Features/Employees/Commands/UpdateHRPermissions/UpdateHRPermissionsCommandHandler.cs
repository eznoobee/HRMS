using HRMS.Application.Common.Exceptions;
using HRMS.Application.Common.Models;
using HRMS.Domain.Entities;
using HRMS.Domain.Enums;
using HRMS.Domain.Interfaces.Repositories;
using HRMS.Domain.Interfaces.Services;
using MediatR;

namespace HRMS.Application.Features.Employees.Commands.UpdateHRPermissions;

public class UpdateHRPermissionsCommandHandler(
    IRepository<Employee> employeeRepo,
    IRepository<EmployeePermission> permissionRepo,
    ICurrentUserService currentUser,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateHRPermissionsCommand, Result>
{
    public async Task<Result> Handle(UpdateHRPermissionsCommand request, CancellationToken ct)
    {
        if (currentUser.Role is not (UserRole.HRManager or UserRole.GeneralManager))
            throw new ForbiddenException("Only HR managers can update HR permissions.");

        var target = await employeeRepo.FirstOrDefaultAsync(
            e => e.Id == request.EmployeeId && !e.IsDeleted, ct)
            ?? throw new NotFoundException(nameof(Employee), request.EmployeeId);

        if (target.CompanyId != currentUser.CompanyId)
            throw new ForbiddenException("You cannot modify employees from another company.");

        if (target.Role != UserRole.HR)
            return Result.Failure("Permissions can only be assigned to HR employees.");

        var existing = await permissionRepo.FindAsync(p => p.EmployeeId == request.EmployeeId, ct);
        foreach (var p in existing)
            permissionRepo.Remove(p);

        foreach (var permission in request.Permissions.Distinct())
        {
            await permissionRepo.AddAsync(new EmployeePermission
            {
                EmployeeId = request.EmployeeId,
                Permission = permission,
                GrantedBy = currentUser.EmployeeId,
                GrantedAt = DateTime.UtcNow
            }, ct);
        }

        await unitOfWork.SaveChangesAsync(ct);
        return Result.Success();
    }
}
