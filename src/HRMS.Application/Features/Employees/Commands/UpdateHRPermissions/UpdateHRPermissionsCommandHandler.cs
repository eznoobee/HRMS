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

        target.Permissions = request.Permissions;
        target.UpdatedAt = DateTime.UtcNow;
        target.UpdatedBy = currentUser.EmployeeId;

        employeeRepo.Update(target);
        await unitOfWork.SaveChangesAsync(ct);

        return Result.Success();
    }
}
