using HRMS.Domain.Entities.Common;
using HRMS.Domain.Enums;
using HRMS.Domain.ValueObjects;


namespace HRMS.Domain.Entities;

public class Employee : AuditableEntity
{
    public string FirstName { get; set; } = default!;
    public string FatherName { get; set; } = default!;
    public string GrandfatherName { get; set; } = default!;
    public string FamilyName { get; set; } = default!;
    public Email Email { get; set; } = default!;
    public PhoneNumber? Phone { get; set; }
    public string? AvatarUrl { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public DateOnly JoinDate { get; set; }
    public string? JobTitle { get; set; }
    public UserRole Role { get; set; }
    public bool IsActive { get; set; } = true;

    public Guid CompanyId { get; set; }
    public Company Company { get; set; } = default!;

    public Guid DepartmentId { get; set; }
    public Department Department { get; set; } = default!;

    public string UserId { get; set; } = default!;

    public ICollection<LeaveRequest> LeaveRequests { get; set; } = [];
    public ICollection<OvertimeRequest> OvertimeRequests { get; set; } = [];
    public ICollection<LeaveBalance> LeaveBalances { get; set; } = [];
    public ICollection<Notification> Notifications { get; set; } = [];
    public ICollection<ChannelMember> ChannelMemberships { get; set; } = [];
    public ICollection<Message> Messages { get; set; } = [];
}
