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
    IRepository<Company> companyRepo,
    IRepository<Department> departmentRepo,
    ICurrentUserService currentUser,
    IUnitOfWork unitOfWork) : IRequestHandler<RegisterCommand, Result<RegisterResponse>>
{
    public async Task<Result<RegisterResponse>> Handle(RegisterCommand request, CancellationToken ct)
    {
        if (currentUser.Role == UserRole.HRManager && request.Role == UserRole.GeneralManager)
            throw new ForbiddenException("HRManagers cannot register a GeneralManager.");

        var company = await companyRepo.GetByIdAsync(request.CompanyId, ct)
            ?? throw new NotFoundException(nameof(Company), request.CompanyId);

        var department = await departmentRepo.GetByIdAsync(request.DepartmentId, ct)
            ?? throw new NotFoundException(nameof(Department), request.DepartmentId);

        if (department.CompanyId != company.Id)
            return Result<RegisterResponse>.Failure("Department does not belong to this company.");

        var emailExists = await employeeRepo.ExistsAsync(e => e.Email == request.Email, ct);
        if (emailExists)
            return Result<RegisterResponse>.Failure("An account with this email already exists.");

        var (userId, error) = await identityService.CreateUserAsync(request.Email, request.Password, request.Role.ToString());
        if (error is not null)
            return Result<RegisterResponse>.Failure(error);

        var employee = new Employee
        {
            FirstName = request.FirstName,
            FatherName = request.FatherName,
            GrandfatherName = request.GrandfatherName,
            FamilyName = request.FamilyName,
            Email = request.Email,
            Phone = PhoneNumber.TryCreate(request.Phone),
            DateOfBirth = request.DateOfBirth,
            JoinDate = request.JoinDate,
            JobTitle = request.JobTitle,
            Role = request.Role,
            CompanyId = request.CompanyId,
            DepartmentId = request.DepartmentId,
            UserId = userId,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = Guid.Empty
        };

        await employeeRepo.AddAsync(employee, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return Result<RegisterResponse>.Success(
            new RegisterResponse(
                employee.Id,
                employee.Email,
                $"{employee.FirstName} {employee.FatherName} {employee.GrandfatherName} {employee.FamilyName}"),
            201);
    }
}
