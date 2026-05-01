using HRMS.Domain.Enums;

namespace HRMS.Domain.Interfaces.Services;

public interface INotificationService
{
    Task SendAsync(Guid employeeId, string title, string body, NotificationType type,
        Guid? relatedEntityId = null, string? actionUrl = null, CancellationToken ct = default);

    Task SendToManyAsync(IEnumerable<Guid> employeeIds, string title, string body, NotificationType type,
        Guid? relatedEntityId = null, string? actionUrl = null, CancellationToken ct = default);
}
