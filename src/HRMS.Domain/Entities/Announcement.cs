using HRMS.Domain.Entities.Common;
using HRMS.Domain.Enums;

namespace HRMS.Domain.Entities;

public class Announcement : AuditableEntity
{
    public string Title { get; set; } = default!;
    public string Body { get; set; } = default!;
    public AnnouncementScope Scope { get; set; }

    public Guid CompanyId { get; set; }
    public Company Company { get; set; } = default!;

    public Guid? DepartmentId { get; set; }
    public Department? Department { get; set; }

    public Guid AuthorId { get; set; }
    public Employee Author { get; set; } = default!;

    public bool IsPinned { get; set; }
    public DateTime? ExpiresAt { get; set; }
}
