using HRMS.Domain.Entities;
using HRMS.Domain.Interfaces.Repositories;
using HRMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Infrastructure.Repositories.Implementations;

public class LeaveTypeRepository(ApplicationDbContext context)
    : Repository<LeaveType>(context), ILeaveTypeRepository
{
    public async Task<IReadOnlyList<LeaveType>> GetActiveByCompanyAsync(Guid companyId, CancellationToken ct = default) =>
        await context.LeaveTypes
            .Where(lt => lt.CompanyId == companyId && lt.IsActive)
            .ToListAsync(ct);

    public async Task<bool> NameExistsInCompanyAsync(string name, Guid companyId, CancellationToken ct = default) =>
        await context.LeaveTypes.AnyAsync(lt => lt.Name == name && lt.CompanyId == companyId, ct);
}
