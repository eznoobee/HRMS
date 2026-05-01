namespace HRMS.Application.DTOs;

public record CompanyDto(
    Guid Id,
    string Name,
    string? Address,
    string? Phone,
    string? Email,
    string? LogoUrl,
    bool IsActive,
    DateTime CreatedAt);
