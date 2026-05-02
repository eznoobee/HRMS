using HRMS.Application.Common.Models;
using HRMS.Application.DTOs;
using HRMS.Domain.Entities;
using HRMS.Domain.Enums;
using HRMS.Domain.Interfaces.Repositories;
using HRMS.Domain.Interfaces.Services;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Application.Features.Employees.Queries.GetEmployees;

public class GetEmployeesQueryHandler(
    IRepository<Employee> employeeRepo,
    ICurrentUserService currentUser) : IRequestHandler<GetEmployeesQuery, Result<PagedResult<EmployeeDto>>>
{
    public async Task<Result<PagedResult<EmployeeDto>>> Handle(GetEmployeesQuery request, CancellationToken ct)
    {
        var isHR = currentUser.Role is UserRole.HR or UserRole.HRManager or UserRole.GeneralManager;
        var isManager = currentUser.Role == UserRole.Manager;

        var query = employeeRepo.Query()
            .Include(e => e.Department)
            .Where(e => e.CompanyId == currentUser.CompanyId);

        // Managers can only see their own department
        if (isManager && !isHR)
            query = query.Where(e => e.DepartmentId == currentUser.DepartmentId);

        if (request.DepartmentId.HasValue)
            query = query.Where(e => e.DepartmentId == request.DepartmentId);

        if (!string.IsNullOrWhiteSpace(request.Role) && Enum.TryParse<UserRole>(request.Role, out var role))
            query = query.Where(e => e.Role == role);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.ToLower();
            query = query.Where(e =>
                e.FirstName.ToLower().Contains(search) ||
                e.FamilyName.ToLower().Contains(search) ||
                e.Email.Value.ToLower().Contains(search));
        }

        var totalCount = await query.CountAsync(ct);
        var items = await query
            .OrderBy(e => e.FirstName).ThenBy(e => e.FamilyName)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectToType<EmployeeDto>()
            .ToListAsync(ct);

        return Result<PagedResult<EmployeeDto>>.Success(new PagedResult<EmployeeDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        });
    }
}
