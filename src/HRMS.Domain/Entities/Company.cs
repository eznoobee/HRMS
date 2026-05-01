using HRMS.Domain.Entities.Common;

namespace HRMS.Domain.Entities;

public class Company : AuditableEntity
{
    public string Name { get; set; } = default!;
    public string? Building { get; set; }
    public string? LogoUrl { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<Department> Departments { get; set; } = [];
    public ICollection<Employee> Employees { get; set; } = [];
    public ICollection<LeaveType> LeaveTypes { get; set; } = [];
    public ICollection<Announcement> Announcements { get; set; } = [];
    public ICollection<Channel> Channels { get; set; } = [];
}
