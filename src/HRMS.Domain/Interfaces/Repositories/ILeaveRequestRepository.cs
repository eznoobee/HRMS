using HRMS.Domain.Entities;
using HRMS.Domain.Enums;

namespace HRMS.Domain.Interfaces.Repositories;

public interface ILeaveRequestRepository : IRepository<LeaveRequest>
{
    Task<LeaveRequest?> GetWithDetailsAsync(Guid leaveRequestId, CancellationToken ct = default);
    Task<IReadOnlyList<LeaveRequest>> GetByEmployeeAsync(Guid employeeId, CancellationToken ct = default);
    Task<IReadOnlyList<LeaveRequest>> GetPendingForDepartmentAsync(Guid departmentId, CancellationToken ct = default);
    Task<IReadOnlyList<LeaveRequest>> GetPendingForHRAsync(Guid companyId, CancellationToken ct = default);
    Task<bool> HasOverlappingRequestAsync(Guid employeeId, DateOnly start, DateOnly end, Guid? excludeId = null, CancellationToken ct = default);
}
