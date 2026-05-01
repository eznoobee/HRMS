using HRMS.Application.Common.Models;
using HRMS.Domain.Enums;
using MediatR;

namespace HRMS.Application.Features.Announcements.Commands.CreateAnnouncement;

public record CreateAnnouncementCommand(
    string Title,
    string Body,
    AnnouncementScope Scope,
    Guid? DepartmentId,
    bool IsPinned,
    DateTime? ExpiresAt
) : IRequest<Result<CreateAnnouncementResponse>>;

public record CreateAnnouncementResponse(Guid AnnouncementId, string Title, string Scope);
