using HRMS.Domain.Entities;

namespace HRMS.Domain.Interfaces.Repositories;

public interface IChannelRepository : IRepository<Channel>
{
    Task<Channel?> GetWithMembersAsync(Guid channelId, CancellationToken ct = default);
    Task<IReadOnlyList<Channel>> GetForEmployeeAsync(Guid employeeId, Guid companyId, CancellationToken ct = default);
    Task<bool> IsMemberAsync(Guid channelId, Guid employeeId, CancellationToken ct = default);
}
