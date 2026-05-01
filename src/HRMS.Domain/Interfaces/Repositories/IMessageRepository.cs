using HRMS.Domain.Entities;

namespace HRMS.Domain.Interfaces.Repositories;

public interface IMessageRepository : IRepository<Message>
{
    Task<IReadOnlyList<Message>> GetByChannelAsync(Guid channelId, int page, int pageSize, CancellationToken ct = default);
    Task<IReadOnlyList<Message>> GetThreadAsync(Guid parentMessageId, CancellationToken ct = default);
}
