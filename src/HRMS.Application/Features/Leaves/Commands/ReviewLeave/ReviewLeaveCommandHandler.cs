using HRMS.Application.Common.Exceptions;
using HRMS.Application.Common.Models;
using HRMS.Domain.Entities;
using HRMS.Domain.Enums;
using HRMS.Domain.Interfaces.Repositories;
using HRMS.Domain.Interfaces.Services;
using MediatR;

namespace HRMS.Application.Features.Leaves.Commands.ReviewLeave;

public class ReviewLeaveCommandHandler(
    IRepository<LeaveRequest> leaveRequestRepo,
    IRepository<LeaveBalance> leaveBalanceRepo,
    IRepository<Employee> employeeRepo,
    INotificationService notificationService,
    ICurrentUserService currentUser,
    IUnitOfWork unitOfWork) : IRequestHandler<ReviewLeaveCommand, Result<ReviewLeaveResponse>>
{
    public async Task<Result<ReviewLeaveResponse>> Handle(ReviewLeaveCommand request, CancellationToken ct)
    {
        var reviewer = await employeeRepo.FirstOrDefaultAsync(
            e => e.Id == currentUser.EmployeeId && !e.IsDeleted, ct)
            ?? throw new NotFoundException(nameof(Employee), currentUser.EmployeeId);

        var leaveRequest = await leaveRequestRepo.FirstOrDefaultAsync(
            lr => lr.Id == request.LeaveRequestId && !lr.IsDeleted, ct)
            ?? throw new NotFoundException(nameof(LeaveRequest), request.LeaveRequestId);

        var isHR = reviewer.Role is UserRole.HR or UserRole.HRManager or UserRole.GeneralManager;
        var isManager = reviewer.Role == UserRole.Manager;

        if (isManager)
        {
            if (leaveRequest.Status != LeaveStatus.Pending)
                return Result<ReviewLeaveResponse>.Failure("This request has already been reviewed by a manager.");

            leaveRequest.ManagerReviewerId = reviewer.Id;
            leaveRequest.ManagerNote = request.Note;
            leaveRequest.ManagerReviewedAt = DateTime.UtcNow;
            leaveRequest.Status = request.IsApproved ? LeaveStatus.ManagerApproved : LeaveStatus.ManagerRejected;
            leaveRequest.UpdatedAt = DateTime.UtcNow;
            leaveRequest.UpdatedBy = reviewer.Id;

            leaveRequestRepo.Update(leaveRequest);
            await unitOfWork.SaveChangesAsync(ct);

            var notifType = request.IsApproved ? NotificationType.LeaveManagerApproved : NotificationType.LeaveManagerRejected;
            await notificationService.SendAsync(
                leaveRequest.EmployeeId,
                request.IsApproved ? "Leave Request Forwarded" : "Leave Request Rejected",
                request.IsApproved
                    ? "Your leave request has been approved by your manager and forwarded to HR."
                    : $"Your leave request was rejected by your manager. Note: {request.Note}",
                notifType, leaveRequest.Id, ct: ct);

            if (request.IsApproved)
            {
                var hrStaff = await employeeRepo.FindAsync(
                    e => e.CompanyId == currentUser.CompanyId &&
                         (e.Role == UserRole.HR || e.Role == UserRole.HRManager) && !e.IsDeleted, ct);

                await notificationService.SendToManyAsync(
                    hrStaff.Select(e => e.Id),
                    "Leave Request Awaiting HR Review",
                    "A leave request has been approved by the manager and is awaiting HR review.",
                    NotificationType.LeaveApplied, leaveRequest.Id, ct: ct);
            }
        }
        else if (isHR)
        {
            if (leaveRequest.Status != LeaveStatus.Pending && leaveRequest.Status != LeaveStatus.ManagerApproved)
                return Result<ReviewLeaveResponse>.Failure("This request is not pending HR review.");

            leaveRequest.HRReviewerId = reviewer.Id;
            leaveRequest.HRNote = request.Note;
            leaveRequest.HRReviewedAt = DateTime.UtcNow;
            leaveRequest.Status = request.IsApproved ? LeaveStatus.HRApproved : LeaveStatus.HRRejected;
            leaveRequest.UpdatedAt = DateTime.UtcNow;
            leaveRequest.UpdatedBy = reviewer.Id;

            leaveRequestRepo.Update(leaveRequest);

            if (request.IsApproved)
            {
                var balance = await leaveBalanceRepo.FirstOrDefaultAsync(
                    b => b.EmployeeId == leaveRequest.EmployeeId &&
                         b.LeaveTypeId == leaveRequest.LeaveTypeId &&
                         b.Year == DateTime.UtcNow.Year, ct);

                if (balance is not null)
                {
                    balance.UsedDays += leaveRequest.TotalDays;
                    leaveBalanceRepo.Update(balance);
                }
            }

            await unitOfWork.SaveChangesAsync(ct);

            var notifType = request.IsApproved ? NotificationType.LeaveHRApproved : NotificationType.LeaveHRRejected;
            await notificationService.SendAsync(
                leaveRequest.EmployeeId,
                request.IsApproved ? "Leave Request Approved" : "Leave Request Rejected",
                request.IsApproved
                    ? "Your leave request has been fully approved."
                    : $"Your leave request was rejected by HR. Note: {request.Note}",
                notifType, leaveRequest.Id, ct: ct);
        }
        else
        {
            throw new ForbiddenException("You are not authorized to review leave requests.");
        }

        return Result<ReviewLeaveResponse>.Success(
            new ReviewLeaveResponse(leaveRequest.Id, leaveRequest.Status.ToString()));
    }
}
