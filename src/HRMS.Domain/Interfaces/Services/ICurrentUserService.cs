using HRMS.Domain.Enums;

namespace HRMS.Domain.Interfaces.Services;

public interface ICurrentUserService
{
    string UserId { get; }
    string Email { get; }
    UserRole Role { get; }
    Guid EmployeeId { get; }
    Guid CompanyId { get; }
    Guid DepartmentId { get; }
    HRPermission Permissions { get; }
    bool IsAuthenticated { get; }
}
