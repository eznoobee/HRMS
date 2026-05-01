using HRMS.Domain.Entities.Common;
using HRMS.Domain.Enums;

namespace HRMS.Domain.Entities;

public class OvertimeRequest : AuditableEntity
{
    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; } = default!;

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public double TotalHours { get; set; }
    public string? Reason { get; set; }
    public OvertimeStatus Status { get; set; } = OvertimeStatus.Pending;

    public Guid? ManagerReviewerId { get; set; }
    public Employee? ManagerReviewer { get; set; }
    public string? ManagerNote { get; set; }
    public DateTime? ManagerReviewedAt { get; set; }

    public Guid? HRReviewerId { get; set; }
    public Employee? HRReviewer { get; set; }
    public string? HRNote { get; set; }
    public DateTime? HRReviewedAt { get; set; }
}
