namespace HRMS.Domain.Enums;

[Flags]
public enum HRPermission
{
    None = 0,
    ViewEmployees = 1,
    RegisterEmployees = 2
}
