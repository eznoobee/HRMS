using HRMS.Domain.Entities;

namespace HRMS.Domain.Interfaces.Repositories;

public interface IOvertimeRequestRepository : IRepository<OvertimeRequest>
{
    Task<OvertimeRequest?> GetWithDetailsAsync(Guid overtimeRequestId, CancellationToken ct = default);
    Task<IReadOnlyList<OvertimeRequest>> GetByEmployeeAsync(Guid employeeId, CancellationToken ct = default);
    Task<IReadOnlyList<OvertimeRequest>> GetPendingForDepartmentAsync(Guid departmentId, CancellationToken ct = default);
    Task<IReadOnlyList<OvertimeRequest>> GetPendingForHRAsync(Guid companyId, CancellationToken ct = default);
}
