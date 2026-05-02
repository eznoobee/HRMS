using HRMS.Application.Common.Exceptions;
using HRMS.Application.Common.Interfaces;
using HRMS.Application.Common.Models;
using HRMS.Domain.Entities;
using HRMS.Domain.Enums;
using HRMS.Domain.Interfaces.Repositories;
using HRMS.Domain.Interfaces.Services;
using HRMS.Domain.ValueObjects;
using MediatR;

namespace HRMS.Application.Features.Employees.Commands.CreateEmployee;

public class CreateEmployeeCommandHandler(
    IIdentityService identityService,
    IRepository<Employee> employeeRepo,
    IRepository<Department> departmentRepo,
    ICurrentUserService currentUser,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateEmployeeCommand, Result>
{
    public async Task<Result> Handle(CreateEmployeeCommand request, CancellationToken ct)
    {
        var isHR = currentUser.Role is UserRole.HR or UserRole.HRManager or UserRole.GeneralManager;
        if (!isHR)
            throw new ForbiddenException("Only HR personnel can create employees.");

        if (currentUser.Role == UserRole.HRManager && request.Role == UserRole.GeneralManager)
            throw new ForbiddenException("HRManagers cannot create a GeneralManager.");

        var department = await departmentRepo.GetByIdAsync(request.DepartmentId, ct)
            ?? throw new NotFoundException(nameof(Department), request.DepartmentId);

        if (department.CompanyId != currentUser.CompanyId)
            return Result.Failure("Department does not belong to your company.");

        var emailExists = await employeeRepo.ExistsAsync(e => e.Email.Value == request.Email.ToLowerInvariant(), ct);
        if (emailExists)
            return Result.Failure("An employee with this email already exists.");

        var (userId, error) = await identityService.CreateUserAsync(
            request.Email, request.Password, request.Role.ToString());
        if (error is not null)
            return Result.Failure(error);

        var employee = new Employee
        {
            FirstName = request.FirstName,
            FatherName = request.FatherName,
            GrandfatherName = request.GrandfatherName,
            FamilyName = request.FamilyName,
            Email = Email.Create(request.Email),
            Phone = PhoneNumber.TryCreate(request.Phone),
            DateOfBirth = request.DateOfBirth,
            JoinDate = request.JoinDate,
            JobTitle = request.JobTitle,
            Role = request.Role,
            CompanyId = currentUser.CompanyId,
            DepartmentId = request.DepartmentId,
            UserId = userId,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = currentUser.EmployeeId
        };

        await employeeRepo.AddAsync(employee, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return Result.Success(201);
    }
}
