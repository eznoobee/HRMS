using HRMS.Domain.Entities;
using HRMS.Domain.Interfaces.Repositories;
using HRMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Infrastructure.Repositories.Implementations;

public class NotificationRepository(ApplicationDbContext context)
    : Repository<Notification>(context), INotificationRepository
{
    public async Task<int> GetUnreadCountAsync(Guid employeeId, CancellationToken ct = default) =>
        await context.Notifications
            .CountAsync(n => n.EmployeeId == employeeId && !n.IsRead, ct);

    public async Task MarkAllReadAsync(Guid employeeId, CancellationToken ct = default) =>
        await context.Notifications
            .Where(n => n.EmployeeId == employeeId && !n.IsRead)
            .ExecuteUpdateAsync(s => s.SetProperty(n => n.IsRead, true), ct);
}
