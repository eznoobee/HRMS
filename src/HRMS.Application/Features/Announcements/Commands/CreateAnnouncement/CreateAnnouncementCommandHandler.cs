using HRMS.Application.Common.Exceptions;
using HRMS.Application.Common.Models;
using HRMS.Domain.Entities;
using HRMS.Domain.Enums;
using HRMS.Domain.Interfaces.Repositories;
using HRMS.Domain.Interfaces.Services;
using MediatR;

namespace HRMS.Application.Features.Announcements.Commands.CreateAnnouncement;

public class CreateAnnouncementCommandHandler(
    IRepository<Announcement> announcementRepo,
    IRepository<Employee> employeeRepo,
    INotificationService notificationService,
    ICurrentUserService currentUser,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateAnnouncementCommand, Result<CreateAnnouncementResponse>>
{
    public async Task<Result<CreateAnnouncementResponse>> Handle(CreateAnnouncementCommand request, CancellationToken ct)
    {
        var author = await employeeRepo.FirstOrDefaultAsync(
            e => e.Id == currentUser.EmployeeId && !e.IsDeleted, ct)
            ?? throw new NotFoundException(nameof(Employee), currentUser.EmployeeId);

        var canAnnounce = author.Role is UserRole.HR or UserRole.HRManager
            or UserRole.GeneralManager or UserRole.Manager;

        if (!canAnnounce)
            throw new ForbiddenException("Only HR, HR Managers, Managers, or General Managers can post announcements.");

        if (request.Scope == AnnouncementScope.Company &&
            author.Role is not (UserRole.HR or UserRole.HRManager or UserRole.GeneralManager))
            throw new ForbiddenException("Only HR or General Managers can post company-wide announcements.");

        var announcement = new Announcement
        {
            Title = request.Title,
            Body = request.Body,
            Scope = request.Scope,
            CompanyId = currentUser.CompanyId,
            DepartmentId = request.DepartmentId,
            AuthorId = author.Id,
            IsPinned = request.IsPinned,
            ExpiresAt = request.ExpiresAt,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = author.Id
        };

        await announcementRepo.AddAsync(announcement, ct);
        await unitOfWork.SaveChangesAsync(ct);

        IEnumerable<Employee> targets;
        if (request.Scope == AnnouncementScope.Department && request.DepartmentId.HasValue)
        {
            targets = await employeeRepo.FindAsync(
                e => e.DepartmentId == request.DepartmentId && !e.IsDeleted, ct);
        }
        else
        {
            targets = await employeeRepo.FindAsync(
                e => e.CompanyId == currentUser.CompanyId && !e.IsDeleted, ct);
        }

        await notificationService.SendToManyAsync(
            targets.Select(e => e.Id).Where(id => id != author.Id),
            announcement.Title,
            $"New announcement: {announcement.Title}",
            NotificationType.NewAnnouncement,
            announcement.Id,
            ct: ct);

        return Result<CreateAnnouncementResponse>.Success(
            new CreateAnnouncementResponse(announcement.Id, announcement.Title, announcement.Scope.ToString()), 201);
    }
}
