using HRMS.Domain.Entities;
using HRMS.Domain.Enums;
using HRMS.Domain.Interfaces.Repositories;
using HRMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Infrastructure.Repositories.Implementations;

public class OvertimeRequestRepository(ApplicationDbContext context)
    : Repository<OvertimeRequest>(context), IOvertimeRequestRepository
{
    public async Task<OvertimeRequest?> GetWithDetailsAsync(Guid id, CancellationToken ct = default) =>
        await context.OvertimeRequests
            .Include(or => or.Employee)
            .Include(or => or.ManagerReviewer)
            .Include(or => or.HRReviewer)
            .FirstOrDefaultAsync(or => or.Id == id, ct);

    public async Task<IReadOnlyList<OvertimeRequest>> GetByEmployeeAsync(Guid employeeId, CancellationToken ct = default) =>
        await context.OvertimeRequests
            .Where(or => or.EmployeeId == employeeId)
            .OrderByDescending(or => or.CreatedAt)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<OvertimeRequest>> GetPendingForDepartmentAsync(Guid departmentId, CancellationToken ct = default) =>
        await context.OvertimeRequests
            .Include(or => or.Employee)
            .Where(or => or.Employee.DepartmentId == departmentId && or.Status == OvertimeStatus.Pending)
            .OrderBy(or => or.CreatedAt)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<OvertimeRequest>> GetPendingForHRAsync(Guid companyId, CancellationToken ct = default) =>
        await context.OvertimeRequests
            .Include(or => or.Employee)
            .Where(or => or.Employee.CompanyId == companyId && or.Status == OvertimeStatus.ManagerApproved)
            .OrderBy(or => or.CreatedAt)
            .ToListAsync(ct);
}
