using HRMS.Application.Common.Exceptions;
using HRMS.Application.Common.Interfaces;
using HRMS.Application.Common.Models;
using HRMS.Domain.Entities;
using HRMS.Domain.Enums;
using HRMS.Domain.Interfaces.Repositories;
using HRMS.Domain.Interfaces.Services;
using HRMS.Domain.ValueObjects;
using MediatR;

namespace HRMS.Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler(
    IIdentityService identityService,
    IRepository<Employee> employeeRepo,
    IRepository<Department> departmentRepo,
    ICurrentUserService currentUser,
    IUnitOfWork unitOfWork) : IRequestHandler<RegisterCommand, Result>
{
    public async Task<Result> Handle(RegisterCommand request, CancellationToken ct)
    {
        var isHRManager = currentUser.Role is UserRole.HRManager or UserRole.GeneralManager;
        var isHRWithPermission = currentUser.Role == UserRole.HR &&
                                 currentUser.HasPermission(HRPermission.RegisterEmployees);

        if (!isHRManager && !isHRWithPermission)
            throw new ForbiddenException("You do not have permission to register employees.");

        if (!isHRManager && request.Role is UserRole.HRManager or UserRole.GeneralManager)
            throw new ForbiddenException("You cannot register HR managers or general managers.");

        var department = await departmentRepo.GetByIdAsync(request.DepartmentId, ct)
            ?? throw new NotFoundException(nameof(Department), request.DepartmentId);

        if (department.CompanyId != currentUser.CompanyId)
            return Result.Failure("Department does not belong to your company.");

        var emailExists = await employeeRepo.ExistsAsync(e => e.Email.Value == request.Email.ToLowerInvariant(), ct);
        if (emailExists)
            return Result.Failure("An account with this email already exists.");

        var (userId, error) = await identityService.CreateUserAsync(request.Email, request.Password, request.Role.ToString());
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
