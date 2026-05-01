namespace HRMS.Application.DTOs;

public record ChannelDto(
    Guid Id,
    string Name,
    string? Description,
    string Type,
    Guid CompanyId,
    Guid? DepartmentId,
    string CreatorFullName,
    int MemberCount,
    DateTime CreatedAt);
