using HRMS.Domain.Entities;

namespace HRMS.Domain.Interfaces.Repositories;

public interface ICompanyRepository : IRepository<Company>
{
    Task<Company?> GetWithDepartmentsAsync(Guid companyId, CancellationToken ct = default);
    Task<bool> NameExistsAsync(string name, CancellationToken ct = default);
}
