using HRMS.Domain.Entities;
using HRMS.Domain.Enums;
using HRMS.Domain.Interfaces.Services;
using HRMS.Infrastructure.Data;
using HRMS.Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Infrastructure.Services;

public class NotificationService(ApplicationDbContext context, IHubContext<NotificationHub> hubContext)
    : INotificationService
{
    public async Task SendAsync(Guid employeeId, string title, string body, NotificationType type,
        Guid? relatedEntityId = null, string? actionUrl = null, CancellationToken ct = default)
    {
        var notification = new Notification
        {
            EmployeeId = employeeId,
            Title = title,
            Body = body,
            Type = type,
            RelatedEntityId = relatedEntityId,
            ActionUrl = actionUrl,
            CreatedAt = DateTime.UtcNow
        };

        context.Notifications.Add(notification);
        await context.SaveChangesAsync(ct);

        var userId = await context.Employees.AsNoTracking()
            .Where(e => e.Id == employeeId)
            .Select(e => e.UserId)
            .FirstOrDefaultAsync(ct);

        if (userId is not null)
        {
            await hubContext.Clients.User(userId).SendAsync("ReceiveNotification", new
            {
                notification.Id,
                notification.Title,
                notification.Body,
                Type = notification.Type.ToString(),
                notification.RelatedEntityId,
                notification.ActionUrl,
                notification.CreatedAt
            }, ct);
        }
    }

    public async Task SendToManyAsync(IEnumerable<Guid> employeeIds, string title, string body,
        NotificationType type, Guid? relatedEntityId = null, string? actionUrl = null, CancellationToken ct = default)
    {
        var ids = employeeIds.ToList();
        var notifications = ids.Select(id => new Notification
        {
            EmployeeId = id,
            Title = title,
            Body = body,
            Type = type,
            RelatedEntityId = relatedEntityId,
            ActionUrl = actionUrl,
            CreatedAt = DateTime.UtcNow
        }).ToList();

        context.Notifications.AddRange(notifications);
        await context.SaveChangesAsync(ct);

        var employeeUserMap = await context.Employees.AsNoTracking()
            .Where(e => ids.Contains(e.Id))
            .Select(e => new { e.Id, e.UserId })
            .ToListAsync(ct);

        foreach (var mapping in employeeUserMap)
        {
            var notification = notifications.First(n => n.EmployeeId == mapping.Id);
            await hubContext.Clients.User(mapping.UserId).SendAsync("ReceiveNotification", new
            {
                notification.Id,
                notification.Title,
                notification.Body,
                Type = notification.Type.ToString(),
                notification.RelatedEntityId,
                notification.ActionUrl,
                notification.CreatedAt
            }, ct);
        }
    }
}
