namespace HRMS.Application.DTOs;

public record MessageDto(
    Guid Id,
    Guid ChannelId,
    Guid SenderId,
    string SenderFullName,
    string? SenderAvatarUrl,
    string Content,
    DateTime SentAt,
    DateTime? EditedAt,
    Guid? ReplyToMessageId,
    string? ReplyToContent);
