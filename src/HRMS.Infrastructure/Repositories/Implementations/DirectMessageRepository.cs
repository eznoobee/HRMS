using HRMS.Domain.Entities;
using HRMS.Domain.Interfaces.Repositories;
using HRMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Infrastructure.Repositories.Implementations;

public class DirectMessageRepository(ApplicationDbContext context)
    : Repository<DirectMessage>(context), IDirectMessageRepository
{
    public async Task<IReadOnlyList<DirectMessage>> GetConversationAsync(Guid employeeAId, Guid employeeBId, int page, int pageSize, CancellationToken ct = default) =>
        await context.DirectMessages
            .Include(dm => dm.Sender)
            .Where(dm =>
                (dm.SenderId == employeeAId && dm.ReceiverId == employeeBId) ||
                (dm.SenderId == employeeBId && dm.ReceiverId == employeeAId))
            .OrderByDescending(dm => dm.SentAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

    public async Task<int> GetUnreadCountAsync(Guid receiverId, CancellationToken ct = default) =>
        await context.DirectMessages
            .CountAsync(dm => dm.ReceiverId == receiverId && !dm.IsRead, ct);
}
