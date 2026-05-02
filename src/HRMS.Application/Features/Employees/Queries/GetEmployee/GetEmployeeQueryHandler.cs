using HRMS.Application.Common.Exceptions;
using HRMS.Application.Common.Models;
using HRMS.Application.DTOs;
using HRMS.Domain.Entities;
using HRMS.Domain.Enums;
using HRMS.Domain.Interfaces.Repositories;
using HRMS.Domain.Interfaces.Services;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Application.Features.Employees.Queries.GetEmployee;

public class GetEmployeeQueryHandler(
    IRepository<Employee> employeeRepo,
    ICurrentUserService currentUser) : IRequestHandler<GetEmployeeQuery, Result<EmployeeDto>>
{
    public async Task<Result<EmployeeDto>> Handle(GetEmployeeQuery request, CancellationToken ct)
    {
        var employee = await employeeRepo.Query()
            .Include(e => e.Department)
            .FirstOrDefaultAsync(e => e.Id == request.EmployeeId && !e.IsDeleted, ct)
            ?? throw new NotFoundException(nameof(Employee), request.EmployeeId);

        // Employees can only view their own profile; HR and above can view any
        var canView = currentUser.EmployeeId == employee.Id
            || currentUser.Role is UserRole.HR or UserRole.HRManager or UserRole.GeneralManager;

        if (!canView)
            throw new ForbiddenException();

        if (employee.CompanyId != currentUser.CompanyId)
            throw new ForbiddenException();

        return Result<EmployeeDto>.Success(employee.Adapt<EmployeeDto>());
    }
}
