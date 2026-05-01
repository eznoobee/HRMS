using HRMS.Domain.Entities;
using HRMS.Domain.Interfaces.Repositories;
using HRMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Infrastructure.Repositories.Implementations;

public class ChannelRepository(ApplicationDbContext context)
    : Repository<Channel>(context), IChannelRepository
{
    public async Task<Channel?> GetWithMembersAsync(Guid channelId, CancellationToken ct = default) =>
        await context.Channels
            .Include(c => c.Members)
            .FirstOrDefaultAsync(c => c.Id == channelId, ct);

    public async Task<IReadOnlyList<Channel>> GetForEmployeeAsync(Guid employeeId, Guid companyId, CancellationToken ct = default) =>
        await context.Channels
            .Where(c => c.CompanyId == companyId && c.Members.Any(m => m.EmployeeId == employeeId))
            .OrderBy(c => c.Name)
            .ToListAsync(ct);

    public async Task<bool> IsMemberAsync(Guid channelId, Guid employeeId, CancellationToken ct = default) =>
        await context.Set<ChannelMember>()
            .AnyAsync(m => m.ChannelId == channelId && m.EmployeeId == employeeId, ct);
}
