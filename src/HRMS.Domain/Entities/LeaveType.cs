using HRMS.Domain.Entities.Common;

namespace HRMS.Domain.Entities;

public class LeaveType : AuditableEntity
{
    public string Name { get; set; } = default!;
    public int MaxDaysPerYear { get; set; }
    public bool IsPaid { get; set; } = true;
    public bool IsActive { get; set; } = true;

    public Guid CompanyId { get; set; }
    public Company Company { get; set; } = default!;

    public ICollection<LeaveRequest> LeaveRequests { get; set; } = [];
    public ICollection<LeaveBalance> LeaveBalances { get; set; } = [];
}
