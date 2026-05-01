using HRMS.Domain.Entities;
using HRMS.Domain.Interfaces.Repositories;
using HRMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Infrastructure.Repositories.Implementations;

public class MessageRepository(ApplicationDbContext context)
    : Repository<Message>(context), IMessageRepository
{
    public async Task<IReadOnlyList<Message>> GetByChannelAsync(Guid channelId, int page, int pageSize, CancellationToken ct = default) =>
        await context.Messages
            .Include(m => m.Sender)
            .Where(m => m.ChannelId == channelId && m.ReplyToMessageId == null)
            .OrderByDescending(m => m.SentAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<Message>> GetThreadAsync(Guid parentMessageId, CancellationToken ct = default) =>
        await context.Messages
            .Include(m => m.Sender)
            .Where(m => m.ReplyToMessageId == parentMessageId)
            .OrderBy(m => m.SentAt)
            .ToListAsync(ct);
}
