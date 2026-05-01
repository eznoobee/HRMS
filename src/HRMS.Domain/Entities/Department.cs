using HRMS.Domain.Entities.Common;

namespace HRMS.Domain.Entities;

public class Department : AuditableEntity
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }

    public Guid CompanyId { get; set; }
    public Company Company { get; set; } = default!;

    public Guid? ManagerId { get; set; }
    public Employee? Manager { get; set; }

    public ICollection<Employee> Employees { get; set; } = [];
    public ICollection<Channel> Channels { get; set; } = [];
    public ICollection<Announcement> Announcements { get; set; } = [];
}
