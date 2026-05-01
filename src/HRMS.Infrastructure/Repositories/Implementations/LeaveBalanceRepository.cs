using HRMS.Domain.Entities;
using HRMS.Domain.Interfaces.Repositories;
using HRMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Infrastructure.Repositories.Implementations;

public class LeaveBalanceRepository(ApplicationDbContext context)
    : Repository<LeaveBalance>(context), ILeaveBalanceRepository
{
    public async Task<LeaveBalance?> GetAsync(Guid employeeId, Guid leaveTypeId, int year, CancellationToken ct = default) =>
        await context.LeaveBalances
            .FirstOrDefaultAsync(lb => lb.EmployeeId == employeeId && lb.LeaveTypeId == leaveTypeId && lb.Year == year, ct);

    public async Task<IReadOnlyList<LeaveBalance>> GetByEmployeeAsync(Guid employeeId, int year, CancellationToken ct = default) =>
        await context.LeaveBalances
            .Include(lb => lb.LeaveType)
            .Where(lb => lb.EmployeeId == employeeId && lb.Year == year)
            .ToListAsync(ct);
}
