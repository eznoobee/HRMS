using HRMS.Domain.Entities;

namespace HRMS.Domain.Interfaces.Repositories;

public interface ILeaveBalanceRepository : IRepository<LeaveBalance>
{
    Task<LeaveBalance?> GetAsync(Guid employeeId, Guid leaveTypeId, int year, CancellationToken ct = default);
    Task<IReadOnlyList<LeaveBalance>> GetByEmployeeAsync(Guid employeeId, int year, CancellationToken ct = default);
}
