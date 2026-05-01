using HRMS.Domain.Entities;

namespace HRMS.Domain.Interfaces.Repositories;

public interface IDirectMessageRepository : IRepository<DirectMessage>
{
    Task<IReadOnlyList<DirectMessage>> GetConversationAsync(Guid employeeAId, Guid employeeBId, int page, int pageSize, CancellationToken ct = default);
    Task<int> GetUnreadCountAsync(Guid receiverId, CancellationToken ct = default);
}
