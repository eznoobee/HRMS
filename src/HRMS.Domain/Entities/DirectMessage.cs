using HRMS.Domain.Entities.Common;

namespace HRMS.Domain.Entities;

public class DirectMessage : BaseEntity
{
    public Guid SenderId { get; set; }
    public Employee Sender { get; set; } = default!;

    public Guid ReceiverId { get; set; }
    public Employee Receiver { get; set; } = default!;

    public string Content { get; set; } = default!;
    public DateTime SentAt { get; set; }
    public DateTime? EditedAt { get; set; }
    public bool IsRead { get; set; }
    public bool IsDeleted { get; set; }
}
