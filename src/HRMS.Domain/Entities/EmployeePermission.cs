using HRMS.Domain.Entities.Common;
using HRMS.Domain.Enums;

namespace HRMS.Domain.Entities;

public class EmployeePermission : BaseEntity
{
    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; } = default!;
    public HRPermission Permission { get; set; }
    public Guid GrantedBy { get; set; }
    public DateTime GrantedAt { get; set; }
}
