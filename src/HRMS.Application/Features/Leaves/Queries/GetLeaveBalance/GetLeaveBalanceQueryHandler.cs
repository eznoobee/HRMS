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

namespace HRMS.Application.Features.Leaves.Queries.GetLeaveBalance;

public class GetLeaveBalanceQueryHandler(
    IRepository<LeaveBalance> leaveBalanceRepo,
    ICurrentUserService currentUser) : IRequestHandler<GetLeaveBalanceQuery, Result<IReadOnlyList<LeaveBalanceDto>>>
{
    public async Task<Result<IReadOnlyList<LeaveBalanceDto>>> Handle(GetLeaveBalanceQuery request, CancellationToken ct)
    {
        var isHR = currentUser.Role is UserRole.HR or UserRole.HRManager or UserRole.GeneralManager;
        var isManager = currentUser.Role == UserRole.Manager;

        if (request.EmployeeId.HasValue && request.EmployeeId.Value != currentUser.EmployeeId)
        {
            if (!isHR && !isManager)
                throw new ForbiddenException("You cannot view another employee's leave balance.");
        }

        var targetEmployeeId = request.EmployeeId ?? currentUser.EmployeeId;
        var year = request.Year ?? DateTime.UtcNow.Year;

        var items = await leaveBalanceRepo.Query()
            .Include(lb => lb.LeaveType)
            .Where(lb => lb.EmployeeId == targetEmployeeId && lb.Year == year)
            .ProjectToType<LeaveBalanceDto>()
            .ToListAsync(ct);

        return Result<IReadOnlyList<LeaveBalanceDto>>.Success(items);
    }
}
