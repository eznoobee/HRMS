using HRMS.Domain.Entities;
using HRMS.Domain.Interfaces.Repositories;
using HRMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Infrastructure.Repositories.Implementations;

public class DepartmentRepository(ApplicationDbContext context)
    : Repository<Department>(context), IDepartmentRepository
{
    public async Task<Department?> GetWithManagerAsync(Guid departmentId, CancellationToken ct = default) =>
        await context.Departments
            .Include(d => d.Manager)
            .FirstOrDefaultAsync(d => d.Id == departmentId, ct);

    public async Task<IReadOnlyList<Department>> GetByCompanyAsync(Guid companyId, CancellationToken ct = default) =>
        await context.Departments
            .Where(d => d.CompanyId == companyId)
            .ToListAsync(ct);

    public async Task<bool> NameExistsInCompanyAsync(string name, Guid companyId, CancellationToken ct = default) =>
        await context.Departments.AnyAsync(d => d.Name == name && d.CompanyId == companyId, ct);
}
