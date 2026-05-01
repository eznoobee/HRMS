using HRMS.Domain.Entities.Common;
using HRMS.Domain.Enums;

namespace HRMS.Domain.Entities;

public class Channel : AuditableEntity
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public ChannelType Type { get; set; }

    public Guid CompanyId { get; set; }
    public Company Company { get; set; } = default!;

    public Guid? DepartmentId { get; set; }
    public Department? Department { get; set; }

    public Guid CreatedById { get; set; }
    public Employee Creator { get; set; } = default!;

    public ICollection<ChannelMember> Members { get; set; } = [];
    public ICollection<Message> Messages { get; set; } = [];
}
