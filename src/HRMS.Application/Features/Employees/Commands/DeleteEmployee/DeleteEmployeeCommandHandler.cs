using HRMS.Application.Common.Exceptions;
using HRMS.Application.Common.Models;
using HRMS.Domain.Entities;
using HRMS.Domain.Enums;
using HRMS.Domain.Interfaces.Repositories;
using HRMS.Domain.Interfaces.Services;
using MediatR;

namespace HRMS.Application.Features.Employees.Commands.DeleteEmployee;

public class DeleteEmployeeCommandHandler(
    IRepository<Employee> employeeRepo,
    ICurrentUserService currentUser,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteEmployeeCommand, Result<DeleteEmployeeResponse>>
{
    public async Task<Result<DeleteEmployeeResponse>> Handle(DeleteEmployeeCommand request, CancellationToken ct)
    {
        var isHR = currentUser.Role is UserRole.HRManager or UserRole.GeneralManager;
        if (!isHR)
            throw new ForbiddenException("Only HR Managers and General Managers can delete employees.");

        if (currentUser.EmployeeId == request.EmployeeId)
            return Result<DeleteEmployeeResponse>.Failure("You cannot delete your own account.");

        var employee = await employeeRepo.FirstOrDefaultAsync(
            e => e.Id == request.EmployeeId && !e.IsDeleted, ct)
            ?? throw new NotFoundException(nameof(Employee), request.EmployeeId);

        if (employee.CompanyId != currentUser.CompanyId)
            throw new ForbiddenException("You cannot delete employees from another company.");

        if (currentUser.Role == UserRole.HRManager && employee.Role == UserRole.GeneralManager)
            throw new ForbiddenException("HRManagers cannot delete a GeneralManager.");

        employee.IsDeleted = true;
        employee.IsActive = false;
        employee.DeletedAt = DateTime.UtcNow;
        employee.UpdatedAt = DateTime.UtcNow;
        employee.UpdatedBy = currentUser.EmployeeId;

        employeeRepo.Update(employee);
        await unitOfWork.SaveChangesAsync(ct);

        return Result<DeleteEmployeeResponse>.Success(new DeleteEmployeeResponse(employee.Id));
    }
}
