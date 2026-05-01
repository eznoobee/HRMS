namespace HRMS.Application.DTOs;

public record EmployeeDto(
    Guid Id,
    string FirstName,
    string FatherName,
    string GrandfatherName,
    string FamilyName,
    string FullName,
    string Email,
    string? Phone,
    string? AvatarUrl,
    DateOnly DateOfBirth,
    DateOnly JoinDate,
    string? JobTitle,
    string Role,
    bool IsActive,
    Guid CompanyId,
    Guid DepartmentId,
    string DepartmentName,
    DateTime CreatedAt);
