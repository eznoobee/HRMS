using HRMS.Domain.Entities;
using HRMS.Domain.Enums;
using HRMS.Domain.Interfaces.Repositories;
using HRMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Infrastructure.Repositories.Implementations;

public class AnnouncementRepository(ApplicationDbContext context)
    : Repository<Announcement>(context), IAnnouncementRepository
{
    public async Task<IReadOnlyList<Announcement>> GetActiveForCompanyAsync(Guid companyId, CancellationToken ct = default) =>
        await context.Announcements
            .Include(a => a.Author)
            .Where(a => a.CompanyId == companyId
                && a.Scope == AnnouncementScope.Company
                && (a.ExpiresAt == null || a.ExpiresAt > DateTime.UtcNow))
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<Announcement>> GetActiveForDepartmentAsync(Guid departmentId, Guid companyId, CancellationToken ct = default) =>
        await context.Announcements
            .Include(a => a.Author)
            .Where(a => a.CompanyId == companyId
                && (a.Scope == AnnouncementScope.Company
                    || (a.Scope == AnnouncementScope.Department && a.DepartmentId == departmentId))
                && (a.ExpiresAt == null || a.ExpiresAt > DateTime.UtcNow))
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync(ct);
}
