using HRMS.Domain.Entities;

namespace HRMS.Domain.Interfaces.Repositories;

public interface IAnnouncementRepository : IRepository<Announcement>
{
    Task<IReadOnlyList<Announcement>> GetActiveForCompanyAsync(Guid companyId, CancellationToken ct = default);
    Task<IReadOnlyList<Announcement>> GetActiveForDepartmentAsync(Guid companyId, Guid departmentId, CancellationToken ct = default);
}
