using HRMS.Domain.Entities;

namespace HRMS.Domain.Interfaces.Repositories;

public interface IDepartmentRepository : IRepository<Department>
{
    Task<Department?> GetWithManagerAsync(Guid departmentId, CancellationToken ct = default);
    Task<IReadOnlyList<Department>> GetByCompanyAsync(Guid companyId, CancellationToken ct = default);
    Task<bool> NameExistsInCompanyAsync(string name, Guid companyId, CancellationToken ct = default);
}
