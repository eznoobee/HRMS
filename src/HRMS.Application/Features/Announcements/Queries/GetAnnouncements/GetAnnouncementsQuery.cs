using HRMS.Application.Common.Models;
using HRMS.Application.DTOs;
using MediatR;

namespace HRMS.Application.Features.Announcements.Queries.GetAnnouncements;

public record GetAnnouncementsQuery(int Page = 1, int PageSize = 20)
    : IRequest<Result<PagedResult<AnnouncementDto>>>;
