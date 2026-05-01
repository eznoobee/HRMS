namespace HRMS.Application.DTOs;

public record NotificationDto(
    Guid Id,
    string Title,
    string Body,
    string Type,
    bool IsRead,
    Guid? RelatedEntityId,
    string? ActionUrl,
    DateTime CreatedAt);
