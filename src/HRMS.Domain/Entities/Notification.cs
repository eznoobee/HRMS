using HRMS.Domain.Entities.Common;
using HRMS.Domain.Enums;

namespace HRMS.Domain.Entities;

public class Notification : BaseEntity
{
    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; } = default!;

    public string Title { get; set; } = default!;
    public string Body { get; set; } = default!;
    public NotificationType Type { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }

    public Guid? RelatedEntityId { get; set; }
    public string? ActionUrl { get; set; }
}
