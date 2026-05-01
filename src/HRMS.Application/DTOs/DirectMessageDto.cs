namespace HRMS.Application.DTOs;

public record DirectMessageDto(
    Guid Id,
    Guid SenderId,
    string SenderFullName,
    string? SenderAvatarUrl,
    Guid ReceiverId,
    string Content,
    DateTime SentAt,
    DateTime? EditedAt,
    bool IsRead);
