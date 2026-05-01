using HRMS.Domain.Entities.Common;
using HRMS.Domain.Enums;

namespace HRMS.Domain.Entities;

public class LeaveRequest : AuditableEntity
{
    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; } = default!;

    public Guid LeaveTypeId { get; set; }
    public LeaveType LeaveType { get; set; } = default!;

    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public int TotalDays { get; set; }
    public string? Reason { get; set; }
    public LeaveStatus Status { get; set; } = LeaveStatus.Pending;

    public Guid? ManagerReviewerId { get; set; }
    public Employee? ManagerReviewer { get; set; }
    public string? ManagerNote { get; set; }
    public DateTime? ManagerReviewedAt { get; set; }

    public Guid? HRReviewerId { get; set; }
    public Employee? HRReviewer { get; set; }
    public string? HRNote { get; set; }
    public DateTime? HRReviewedAt { get; set; }
}
