using HRMS.Domain.Entities;
using HRMS.Domain.Enums;
using HRMS.Domain.Interfaces.Repositories;
using HRMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Infrastructure.Repositories.Implementations;

public class LeaveRequestRepository(ApplicationDbContext context)
    : Repository<LeaveRequest>(context), ILeaveRequestRepository
{
    public async Task<LeaveRequest?> GetWithDetailsAsync(Guid id, CancellationToken ct = default) =>
        await context.LeaveRequests
            .Include(lr => lr.Employee)
            .Include(lr => lr.LeaveType)
            .Include(lr => lr.ManagerReviewer)
            .Include(lr => lr.HRReviewer)
            .FirstOrDefaultAsync(lr => lr.Id == id, ct);

    public async Task<IReadOnlyList<LeaveRequest>> GetByEmployeeAsync(Guid employeeId, CancellationToken ct = default) =>
        await context.LeaveRequests
            .Include(lr => lr.LeaveType)
            .Where(lr => lr.EmployeeId == employeeId)
            .OrderByDescending(lr => lr.CreatedAt)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<LeaveRequest>> GetPendingForDepartmentAsync(Guid departmentId, CancellationToken ct = default) =>
        await context.LeaveRequests
            .Include(lr => lr.Employee)
            .Include(lr => lr.LeaveType)
            .Where(lr => lr.Employee.DepartmentId == departmentId && lr.Status == LeaveStatus.Pending)
            .OrderBy(lr => lr.CreatedAt)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<LeaveRequest>> GetPendingForHRAsync(Guid companyId, CancellationToken ct = default) =>
        await context.LeaveRequests
            .Include(lr => lr.Employee)
            .Include(lr => lr.LeaveType)
            .Where(lr => lr.Employee.CompanyId == companyId && lr.Status == LeaveStatus.ManagerApproved)
            .OrderBy(lr => lr.CreatedAt)
            .ToListAsync(ct);

    public async Task<bool> HasOverlappingRequestAsync(Guid employeeId, DateOnly startDate, DateOnly endDate, Guid? excludeId = null, CancellationToken ct = default) =>
        await context.LeaveRequests
            .Where(lr => lr.EmployeeId == employeeId
                && (excludeId == null || lr.Id != excludeId)
                && lr.Status != LeaveStatus.ManagerRejected
                && lr.Status != LeaveStatus.HRRejected
                && lr.Status != LeaveStatus.Cancelled
                && lr.StartDate <= endDate
                && lr.EndDate >= startDate)
            .AnyAsync(ct);
}
