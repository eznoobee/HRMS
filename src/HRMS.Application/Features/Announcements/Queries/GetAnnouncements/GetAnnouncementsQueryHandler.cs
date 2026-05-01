using HRMS.Application.Common.Models;
using HRMS.Application.DTOs;
using HRMS.Domain.Entities;
using HRMS.Domain.Interfaces.Repositories;
using HRMS.Domain.Interfaces.Services;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Application.Features.Announcements.Queries.GetAnnouncements;

public class GetAnnouncementsQueryHandler(
    IRepository<Announcement> announcementRepo,
    ICurrentUserService currentUser) : IRequestHandler<GetAnnouncementsQuery, Result<PagedResult<AnnouncementDto>>>
{
    public async Task<Result<PagedResult<AnnouncementDto>>> Handle(GetAnnouncementsQuery request, CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        var query = announcementRepo.Query()
            .Include(a => a.Author)
            .Include(a => a.Department)
            .Where(a => a.CompanyId == currentUser.CompanyId)
            .Where(a => a.ExpiresAt == null || a.ExpiresAt > now);

        var totalCount = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(a => a.IsPinned)
            .ThenByDescending(a => a.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectToType<AnnouncementDto>()
            .ToListAsync(ct);

        return Result<PagedResult<AnnouncementDto>>.Success(new PagedResult<AnnouncementDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        });
    }
}
