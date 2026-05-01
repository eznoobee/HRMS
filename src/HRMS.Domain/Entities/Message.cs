using HRMS.Domain.Entities.Common;

namespace HRMS.Domain.Entities;

public class Message : BaseEntity
{
    public Guid ChannelId { get; set; }
    public Channel Channel { get; set; } = default!;

    public Guid SenderId { get; set; }
    public Employee Sender { get; set; } = default!;

    public string Content { get; set; } = default!;
    public DateTime SentAt { get; set; }
    public DateTime? EditedAt { get; set; }
    public bool IsDeleted { get; set; }

    public Guid? ReplyToMessageId { get; set; }
    public Message? ReplyToMessage { get; set; }
}
