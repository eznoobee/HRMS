using HRMS.Domain.Entities;
using HRMS.Domain.Enums;
using HRMS.Domain.Interfaces.Repositories;
using HRMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Infrastructure.Repositories.Implementations;

public class EmployeeRepository(ApplicationDbContext context)
    : Repository<Employee>(context), IEmployeeRepository
{
    public async Task<Employee?> GetByUserIdAsync(string userId, CancellationToken ct = default) =>
        await context.Employees
            .FirstOrDefaultAsync(e => e.UserId == userId, ct);

    public async Task<Employee?> GetWithDepartmentAsync(Guid employeeId, CancellationToken ct = default) =>
        await context.Employees
            .Include(e => e.Department)
            .FirstOrDefaultAsync(e => e.Id == employeeId, ct);

    public async Task<IReadOnlyList<Employee>> GetByCompanyAsync(Guid companyId, CancellationToken ct = default) =>
        await context.Employees
            .Where(e => e.CompanyId == companyId)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<Employee>> GetByDepartmentAsync(Guid departmentId, CancellationToken ct = default) =>
        await context.Employees
            .Where(e => e.DepartmentId == departmentId)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<Employee>> GetByRoleInCompanyAsync(Guid companyId, UserRole role, CancellationToken ct = default) =>
        await context.Employees
            .Where(e => e.Role == role && e.CompanyId == companyId)
            .ToListAsync(ct);

    public async Task<bool> EmailExistsAsync(string email, CancellationToken ct = default) =>
        await context.Employees.AnyAsync(e => e.Email == email, ct);
}
