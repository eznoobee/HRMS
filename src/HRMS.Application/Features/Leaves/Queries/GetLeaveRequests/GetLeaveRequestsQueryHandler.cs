using HRMS.Application.Common.Models;
using HRMS.Application.DTOs;
using HRMS.Domain.Entities;
using HRMS.Domain.Enums;
using HRMS.Domain.Interfaces.Repositories;
using HRMS.Domain.Interfaces.Services;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Application.Features.Leaves.Queries.GetLeaveRequests;

public class GetLeaveRequestsQueryHandler(
    IRepository<LeaveRequest> leaveRequestRepo,
    ICurrentUserService currentUser) : IRequestHandler<GetLeaveRequestsQuery, Result<PagedResult<LeaveRequestDto>>>
{
    public async Task<Result<PagedResult<LeaveRequestDto>>> Handle(GetLeaveRequestsQuery request, CancellationToken ct)
    {
        var isHR = currentUser.Role is UserRole.HR or UserRole.HRManager or UserRole.GeneralManager;
        var isManager = currentUser.Role == UserRole.Manager;

        var query = leaveRequestRepo.Query()
            .Include(lr => lr.Employee).ThenInclude(e => e.Department)
            .Include(lr => lr.LeaveType)
            .Include(lr => lr.ManagerReviewer)
            .Include(lr => lr.HRReviewer)
            .Where(lr => lr.Employee.CompanyId == currentUser.CompanyId);

        // Employees see only their own; managers see their dept; HR sees all
        if (!isHR && !isManager)
            query = query.Where(lr => lr.EmployeeId == currentUser.EmployeeId);
        else if (isManager && !isHR)
            query = query.Where(lr => lr.Employee.DepartmentId == currentUser.DepartmentId);

        if (request.EmployeeId.HasValue && (isHR || isManager))
            query = query.Where(lr => lr.EmployeeId == request.EmployeeId);

        if (!string.IsNullOrWhiteSpace(request.Status) && Enum.TryParse<LeaveStatus>(request.Status, out var status))
            query = query.Where(lr => lr.Status == status);

        var totalCount = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(lr => lr.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectToType<LeaveRequestDto>()
            .ToListAsync(ct);

        return Result<PagedResult<LeaveRequestDto>>.Success(new PagedResult<LeaveRequestDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        });
    }
}
