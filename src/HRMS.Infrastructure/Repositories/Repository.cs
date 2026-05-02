using System.Linq.Expressions;
using HRMS.Domain.Entities.Common;
using HRMS.Domain.Interfaces.Repositories;
using HRMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Infrastructure.Repositories;

public class Repository<T>(ApplicationDbContext context) : IRepository<T> where T : BaseEntity
{
    private readonly DbSet<T> _set = context.Set<T>();

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await _set.FindAsync([id], ct);

    public async Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default) =>
        await _set.Where(predicate).ToListAsync(ct);

    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default) =>
        await _set.FirstOrDefaultAsync(predicate, ct);

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default) =>
        await _set.AnyAsync(predicate, ct);

    public async Task AddAsync(T entity, CancellationToken ct = default) =>
        await _set.AddAsync(entity, ct);

    public void Update(T entity) =>
        _set.Update(entity);

    public void Remove(T entity) =>
        _set.Remove(entity);

    public IQueryable<T> Query() =>
        _set.AsQueryable();
}
