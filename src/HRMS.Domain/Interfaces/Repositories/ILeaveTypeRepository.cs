using HRMS.Domain.Entities;

namespace HRMS.Domain.Interfaces.Repositories;

public interface ILeaveTypeRepository : IRepository<LeaveType>
{
    Task<IReadOnlyList<LeaveType>> GetActiveByCompanyAsync(Guid companyId, CancellationToken ct = default);
    Task<bool> NameExistsInCompanyAsync(string name, Guid companyId, CancellationToken ct = default);
}
