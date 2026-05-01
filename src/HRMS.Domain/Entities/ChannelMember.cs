using HRMS.Domain.Entities.Common;

namespace HRMS.Domain.Entities;

public class ChannelMember : BaseEntity
{
    public Guid ChannelId { get; set; }
    public Channel Channel { get; set; } = default!;

    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; } = default!;

    public DateTime JoinedAt { get; set; }
    public bool IsAdmin { get; set; }
}
