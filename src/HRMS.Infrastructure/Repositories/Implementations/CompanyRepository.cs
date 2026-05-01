using HRMS.Domain.Entities;
using HRMS.Domain.Interfaces.Repositories;
using HRMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Infrastructure.Repositories.Implementations;

public class CompanyRepository(ApplicationDbContext context)
    : Repository<Company>(context), ICompanyRepository
{
    public async Task<Company?> GetWithDepartmentsAsync(Guid companyId, CancellationToken ct = default) =>
        await context.Companies
            .Include(c => c.Departments)
            .FirstOrDefaultAsync(c => c.Id == companyId, ct);

    public async Task<bool> NameExistsAsync(string name, CancellationToken ct = default) =>
        await context.Companies.AnyAsync(c => c.Name == name, ct);
}
