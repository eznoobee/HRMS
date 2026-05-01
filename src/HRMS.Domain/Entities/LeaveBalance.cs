using HRMS.Domain.Entities.Common;

namespace HRMS.Domain.Entities;

public class LeaveBalance : BaseEntity
{
    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; } = default!;

    public Guid LeaveTypeId { get; set; }
    public LeaveType LeaveType { get; set; } = default!;

    public int Year { get; set; }
    public int TotalDays { get; set; }
    public int UsedDays { get; set; }
    public int RemainingDays => TotalDays - UsedDays;
}
