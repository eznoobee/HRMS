using HRMS.Application.Common.Exceptions;
using HRMS.Application.Common.Models;
using HRMS.Domain.Entities;
using HRMS.Domain.Enums;
using HRMS.Domain.Interfaces.Repositories;
using HRMS.Domain.Interfaces.Services;
using MediatR;

namespace HRMS.Application.Features.Leaves.Commands.ApplyLeave;

public class ApplyLeaveCommandHandler(
    IRepository<LeaveRequest> leaveRequestRepo,
    IRepository<LeaveBalance> leaveBalanceRepo,
    IRepository<Employee> employeeRepo,
    IRepository<LeaveType> leaveTypeRepo,
    INotificationService notificationService,
    ICurrentUserService currentUser,
    IUnitOfWork unitOfWork) : IRequestHandler<ApplyLeaveCommand, Result<ApplyLeaveResponse>>
{
    public async Task<Result<ApplyLeaveResponse>> Handle(ApplyLeaveCommand request, CancellationToken ct)
    {
        var employee = await employeeRepo.FirstOrDefaultAsync(
            e => e.Id == currentUser.EmployeeId && !e.IsDeleted, ct)
            ?? throw new NotFoundException(nameof(Employee), currentUser.EmployeeId);

        var leaveType = await leaveTypeRepo.FirstOrDefaultAsync(
            lt => lt.Id == request.LeaveTypeId && lt.CompanyId == currentUser.CompanyId && lt.IsActive, ct)
            ?? throw new NotFoundException(nameof(LeaveType), request.LeaveTypeId);

        var totalDays = request.EndDate.DayNumber - request.StartDate.DayNumber + 1;

        var balance = await leaveBalanceRepo.FirstOrDefaultAsync(
            b => b.EmployeeId == employee.Id &&
                 b.LeaveTypeId == request.LeaveTypeId &&
                 b.Year == DateTime.UtcNow.Year, ct);

        if (balance is null || balance.RemainingDays < totalDays)
            return Result<ApplyLeaveResponse>.Failure($"Insufficient leave balance. Remaining: {balance?.RemainingDays ?? 0} days.");

        var isManager = employee.Role == UserRole.Manager ||
                        employee.Role == UserRole.HRManager ||
                        employee.Role == UserRole.HR ||
                        employee.Role == UserRole.GeneralManager;

        var leaveRequest = new LeaveRequest
        {
            EmployeeId = employee.Id,
            LeaveTypeId = request.LeaveTypeId,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            TotalDays = totalDays,
            Reason = request.Reason,
            Status = LeaveStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = employee.Id
        };

        await leaveRequestRepo.AddAsync(leaveRequest, ct);
        await unitOfWork.SaveChangesAsync(ct);

        if (isManager)
        {
            var hrEmployees = await employeeRepo.FindAsync(
                e => e.CompanyId == currentUser.CompanyId &&
                     (e.Role == UserRole.HR || e.Role == UserRole.HRManager) &&
                     !e.IsDeleted, ct);

            await notificationService.SendToManyAsync(
                hrEmployees.Select(e => e.Id),
                "New Leave Request",
                $"{employee.FirstName} {employee.FamilyName} has submitted a leave request for {totalDays} day(s).",
                NotificationType.LeaveApplied,
                leaveRequest.Id,
                ct: ct);
        }
        else
        {
            var manager = await employeeRepo.FirstOrDefaultAsync(
                e => e.DepartmentId == employee.DepartmentId &&
                     e.Role == UserRole.Manager && !e.IsDeleted, ct);

            if (manager is not null)
            {
                await notificationService.SendAsync(
                    manager.Id,
                    "New Leave Request",
                    $"{employee.FirstName} {employee.FamilyName} has submitted a leave request for {totalDays} day(s).",
                    NotificationType.LeaveApplied,
                    leaveRequest.Id,
                    ct: ct);
            }
        }

        return Result<ApplyLeaveResponse>.Success(
            new ApplyLeaveResponse(leaveRequest.Id, leaveRequest.Status.ToString(), totalDays), 201);
    }
}
