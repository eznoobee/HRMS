using HRMS.Application.Common.Models;
using HRMS.Application.DTOs;
using MediatR;

namespace HRMS.Application.Features.Notifications.Queries.GetNotifications;

public record GetNotificationsQuery(bool UnreadOnly = false, int Page = 1, int PageSize = 30)
    : IRequest<Result<PagedResult<NotificationDto>>>;
