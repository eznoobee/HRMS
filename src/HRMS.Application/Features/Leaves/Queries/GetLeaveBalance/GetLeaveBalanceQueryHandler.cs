using HRMS.Application.Common.Models;
using HRMS.Application.DTOs;
using HRMS.Domain.Entities;
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
