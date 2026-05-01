namespace HRMS.Application.DTOs;

public record DepartmentDto(
    Guid Id,
    string Name,
    string? Description,
    Guid CompanyId,
    Guid? ManagerId,
    string? ManagerFullName,
    DateTime CreatedAt);
