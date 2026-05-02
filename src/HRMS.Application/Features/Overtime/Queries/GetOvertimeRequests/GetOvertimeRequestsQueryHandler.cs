using HRMS.Application.Common.Models;
using HRMS.Application.DTOs;
using HRMS.Domain.Entities;
using HRMS.Domain.Enums;
using HRMS.Domain.Interfaces.Repositories;
using HRMS.Domain.Interfaces.Services;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Application.Features.Overtime.Queries.GetOvertimeRequests;

public class GetOvertimeRequestsQueryHandler(
    IRepository<OvertimeRequest> overtimeRepo,
    IRepository<Employee> employeeRepo,
    ICurrentUserService currentUser) : IRequestHandler<GetOvertimeRequestsQuery, Result<PagedResult<OvertimeRequestDto>>>
{
    public async Task<Result<PagedResult<OvertimeRequestDto>>> Handle(GetOvertimeRequestsQuery request, CancellationToken ct)
    {
        var isHR = currentUser.Role is UserRole.HR or UserRole.HRManager or UserRole.GeneralManager;
        var isManager = currentUser.Role == UserRole.Manager;

        var query = overtimeRepo.Query()
            .Include(o => o.Employee)
            .Include(o => o.ManagerReviewer)
            .Include(o => o.HRReviewer)
            .Where(o => !o.IsDeleted);

        if (isHR)
        {
            query = query.Where(o => o.Employee.CompanyId == currentUser.CompanyId);
        }
        else if (isManager)
        {
            var managerEmployee = await employeeRepo.FirstOrDefaultAsync(
                e => e.Id == currentUser.EmployeeId && !e.IsDeleted, ct);
            var deptId = managerEmployee?.DepartmentId;
            query = query.Where(o => o.Employee.DepartmentId == deptId);
        }
        else
        {
            query = query.Where(o => o.EmployeeId == currentUser.EmployeeId);
        }

        if (request.EmployeeId.HasValue && (isHR || isManager))
            query = query.Where(o => o.EmployeeId == request.EmployeeId);

        if (!string.IsNullOrWhiteSpace(request.Status) &&
            Enum.TryParse<OvertimeStatus>(request.Status, out var status))
            query = query.Where(o => o.Status == status);

        var totalCount = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(o => o.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectToType<OvertimeRequestDto>()
            .ToListAsync(ct);

        return Result<PagedResult<OvertimeRequestDto>>.Success(new PagedResult<OvertimeRequestDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        });
    }
}
