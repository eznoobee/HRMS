using HRMS.Application.Common.Exceptions;
using HRMS.Application.Common.Models;
using HRMS.Domain.Entities;
using HRMS.Domain.Enums;
using HRMS.Domain.Interfaces.Repositories;
using HRMS.Domain.Interfaces.Services;
using MediatR;

namespace HRMS.Application.Features.Employees.Commands.UpdateEmployee;

public class UpdateEmployeeCommandHandler(
    IRepository<Employee> employeeRepo,
    IRepository<Department> departmentRepo,
    ICurrentUserService currentUser,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateEmployeeCommand, Result<UpdateEmployeeResponse>>
{
    public async Task<Result<UpdateEmployeeResponse>> Handle(UpdateEmployeeCommand request, CancellationToken ct)
    {
        var isHR = currentUser.Role is UserRole.HR or UserRole.HRManager or UserRole.GeneralManager;
        var isSelf = currentUser.EmployeeId == request.EmployeeId;

        if (!isHR && !isSelf)
            throw new ForbiddenException("You can only update your own profile.");

        var employee = await employeeRepo.FirstOrDefaultAsync(
            e => e.Id == request.EmployeeId && !e.IsDeleted, ct)
            ?? throw new NotFoundException(nameof(Employee), request.EmployeeId);

        if (employee.CompanyId != currentUser.CompanyId)
            throw new ForbiddenException("You cannot modify employees from another company.");

        if (isHR)
        {
            var department = await departmentRepo.GetByIdAsync(request.DepartmentId, ct)
                ?? throw new NotFoundException(nameof(Department), request.DepartmentId);

            if (department.CompanyId != currentUser.CompanyId)
                return Result<UpdateEmployeeResponse>.Failure("Department does not belong to your company.");

            if (currentUser.Role == UserRole.HRManager && request.Role == UserRole.GeneralManager)
                throw new ForbiddenException("HRManagers cannot assign the GeneralManager role.");

            employee.Role = request.Role;
            employee.DepartmentId = request.DepartmentId;
            employee.JoinDate = request.JoinDate;
            employee.IsActive = request.IsActive;
        }

        employee.FirstName = request.FirstName;
        employee.LastName = request.LastName;
        employee.Phone = request.Phone;
        employee.AvatarUrl = request.AvatarUrl;
        employee.DateOfBirth = request.DateOfBirth;
        employee.JobTitle = request.JobTitle;
        employee.UpdatedAt = DateTime.UtcNow;
        employee.UpdatedBy = currentUser.EmployeeId;

        employeeRepo.Update(employee);
        await unitOfWork.SaveChangesAsync(ct);

        return Result<UpdateEmployeeResponse>.Success(
            new UpdateEmployeeResponse(employee.Id, $"{employee.FirstName} {employee.LastName}"));
    }
}
