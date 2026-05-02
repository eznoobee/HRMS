using System.Security.Claims;
using HRMS.Domain.Entities;
using HRMS.Domain.Enums;
using HRMS.Domain.Interfaces.Services;
using HRMS.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Infrastructure.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
    : ICurrentUserService
{
    private const string CacheKey = "CurrentEmployee";

    private ClaimsPrincipal? User => httpContextAccessor.HttpContext?.User;

    public string UserId =>
        User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

    public string Email =>
        User?.FindFirstValue(ClaimTypes.Email) ?? string.Empty;

    public UserRole Role =>
        Enum.TryParse<UserRole>(User?.FindFirstValue(ClaimTypes.Role), out var role)
            ? role : UserRole.Employee;

    public bool IsAuthenticated => User?.Identity?.IsAuthenticated == true;

    public Guid EmployeeId => GetEmployee()?.Id ?? Guid.Empty;
    public Guid CompanyId => GetEmployee()?.CompanyId ?? Guid.Empty;
    public Guid DepartmentId => GetEmployee()?.DepartmentId ?? Guid.Empty;
    public bool HasPermission(HRPermission permission) =>
        GetEmployee()?.GrantedPermissions.Any(p => p.Permission == permission) == true;

    private Employee? GetEmployee()
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext is null || !IsAuthenticated) return null;

        if (httpContext.Items.TryGetValue(CacheKey, out var cached))
            return cached as Employee;

        var employee = context.Employees
            .Include(e => e.GrantedPermissions)
            .AsNoTracking()
            .FirstOrDefault(e => e.UserId == UserId && !e.IsDeleted);

        httpContext.Items[CacheKey] = employee;
        return employee;
    }
}
