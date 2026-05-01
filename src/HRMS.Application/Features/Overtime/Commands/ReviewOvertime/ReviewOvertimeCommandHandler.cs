using HRMS.Application.Common.Exceptions;
using HRMS.Application.Common.Models;
using HRMS.Domain.Entities;
using HRMS.Domain.Enums;
using HRMS.Domain.Interfaces.Repositories;
using HRMS.Domain.Interfaces.Services;
using MediatR;

namespace HRMS.Application.Features.Overtime.Commands.ReviewOvertime;

public class ReviewOvertimeCommandHandler(
    IRepository<OvertimeRequest> overtimeRepo,
    IRepository<Employee> employeeRepo,
    INotificationService notificationService,
    ICurrentUserService currentUser,
    IUnitOfWork unitOfWork) : IRequestHandler<ReviewOvertimeCommand, Result<ReviewOvertimeResponse>>
{
    public async Task<Result<ReviewOvertimeResponse>> Handle(ReviewOvertimeCommand request, CancellationToken ct)
    {
        var reviewer = await employeeRepo.FirstOrDefaultAsync(
            e => e.Id == currentUser.EmployeeId && !e.IsDeleted, ct)
            ?? throw new NotFoundException(nameof(Employee), currentUser.EmployeeId);

        var overtimeRequest = await overtimeRepo.FirstOrDefaultAsync(
            o => o.Id == request.OvertimeRequestId && !o.IsDeleted, ct)
            ?? throw new NotFoundException(nameof(OvertimeRequest), request.OvertimeRequestId);

        var isHR = reviewer.Role is UserRole.HR or UserRole.HRManager or UserRole.GeneralManager;
        var isManager = reviewer.Role == UserRole.Manager;

        if (isManager)
        {
            if (overtimeRequest.Status != OvertimeStatus.Pending)
                return Result<ReviewOvertimeResponse>.Failure("This request has already been reviewed by a manager.");

            overtimeRequest.ManagerReviewerId = reviewer.Id;
            overtimeRequest.ManagerNote = request.Note;
            overtimeRequest.ManagerReviewedAt = DateTime.UtcNow;
            overtimeRequest.Status = request.IsApproved ? OvertimeStatus.ManagerApproved : OvertimeStatus.ManagerRejected;
            overtimeRequest.UpdatedAt = DateTime.UtcNow;
            overtimeRequest.UpdatedBy = reviewer.Id;

            overtimeRepo.Update(overtimeRequest);
            await unitOfWork.SaveChangesAsync(ct);

            var notifType = request.IsApproved
                ? NotificationType.OvertimeManagerApproved
                : NotificationType.OvertimeManagerRejected;

            await notificationService.SendAsync(
                overtimeRequest.EmployeeId,
                request.IsApproved ? "Overtime Request Forwarded" : "Overtime Request Rejected",
                request.IsApproved
                    ? "Your overtime request has been approved by your manager and forwarded to HR."
                    : $"Your overtime request was rejected by your manager. Note: {request.Note}",
                notifType, overtimeRequest.Id, ct: ct);

            if (request.IsApproved)
            {
                var hrStaff = await employeeRepo.FindAsync(
                    e => e.CompanyId == currentUser.CompanyId &&
                         (e.Role == UserRole.HR || e.Role == UserRole.HRManager) && !e.IsDeleted, ct);

                await notificationService.SendToManyAsync(
                    hrStaff.Select(e => e.Id),
                    "Overtime Request Awaiting HR Review",
                    "An overtime request has been approved by the manager and is awaiting HR review.",
                    NotificationType.OvertimeApplied, overtimeRequest.Id, ct: ct);
            }
        }
        else if (isHR)
        {
            if (overtimeRequest.Status != OvertimeStatus.Pending && overtimeRequest.Status != OvertimeStatus.ManagerApproved)
                return Result<ReviewOvertimeResponse>.Failure("This request is not pending HR review.");

            overtimeRequest.HRReviewerId = reviewer.Id;
            overtimeRequest.HRNote = request.Note;
            overtimeRequest.HRReviewedAt = DateTime.UtcNow;
            overtimeRequest.Status = request.IsApproved ? OvertimeStatus.HRApproved : OvertimeStatus.HRRejected;
            overtimeRequest.UpdatedAt = DateTime.UtcNow;
            overtimeRequest.UpdatedBy = reviewer.Id;

            overtimeRepo.Update(overtimeRequest);
            await unitOfWork.SaveChangesAsync(ct);

            var notifType = request.IsApproved
                ? NotificationType.OvertimeHRApproved
                : NotificationType.OvertimeHRRejected;

            await notificationService.SendAsync(
                overtimeRequest.EmployeeId,
                request.IsApproved ? "Overtime Request Approved" : "Overtime Request Rejected",
                request.IsApproved
                    ? "Your overtime request has been fully approved."
                    : $"Your overtime request was rejected by HR. Note: {request.Note}",
                notifType, overtimeRequest.Id, ct: ct);
        }
        else
        {
            throw new ForbiddenException("You are not authorized to review overtime requests.");
        }

        return Result<ReviewOvertimeResponse>.Success(
            new ReviewOvertimeResponse(overtimeRequest.Id, overtimeRequest.Status.ToString()));
    }
}
