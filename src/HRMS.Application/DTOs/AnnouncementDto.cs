namespace HRMS.Application.DTOs;

public record AnnouncementDto(
    Guid Id,
    string Title,
    string Body,
    string Scope,
    Guid CompanyId,
    Guid? DepartmentId,
    string? DepartmentName,
    string AuthorFullName,
    bool IsPinned,
    DateTime? ExpiresAt,
    DateTime CreatedAt);
