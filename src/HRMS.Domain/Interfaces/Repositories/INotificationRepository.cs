using HRMS.Domain.Entities;

namespace HRMS.Domain.Interfaces.Repositories;

public interface INotificationRepository : IRepository<Notification>
{
    Task<int> GetUnreadCountAsync(Guid employeeId, CancellationToken ct = default);
    Task MarkAllReadAsync(Guid employeeId, CancellationToken ct = default);
}
