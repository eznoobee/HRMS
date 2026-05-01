using HRMS.Application.Common.Exceptions;
using HRMS.Application.Common.Models;
using HRMS.Domain.Entities;
using HRMS.Domain.Enums;
using HRMS.Domain.Interfaces.Repositories;
using HRMS.Domain.Interfaces.Services;
using MediatR;

namespace HRMS.Application.Features.Overtime.Commands.ApplyOvertime;

public class ApplyOvertimeCommandHandler(
    IRepository<OvertimeRequest> overtimeRepo,
    IRepository<Employee> employeeRepo,
    INotificationService notificationService,
    ICurrentUserService currentUser,
    IUnitOfWork unitOfWork) : IRequestHandler<ApplyOvertimeCommand, Result<ApplyOvertimeResponse>>
{
    public async Task<Result<ApplyOvertimeResponse>> Handle(ApplyOvertimeCommand request, CancellationToken ct)
    {
        var employee = await employeeRepo.FirstOrDefaultAsync(
            e => e.Id == currentUser.EmployeeId && !e.IsDeleted, ct)
            ?? throw new NotFoundException(nameof(Employee), currentUser.EmployeeId);

        var totalHours = (request.EndTime - request.StartTime).TotalHours;
        if (totalHours <= 0)
            return Result<ApplyOvertimeResponse>.Failure("Overtime duration must be greater than zero.");

        var overtimeRequest = new OvertimeRequest
        {
            EmployeeId = employee.Id,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            TotalHours = totalHours,
            Reason = request.Reason,
            Status = OvertimeStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = employee.Id
        };

        await overtimeRepo.AddAsync(overtimeRequest, ct);
        await unitOfWork.SaveChangesAsync(ct);

        var isManager = employee.Role is UserRole.Manager or UserRole.HRManager
            or UserRole.HR or UserRole.GeneralManager;

        if (isManager)
        {
            var hrStaff = await employeeRepo.FindAsync(
                e => e.CompanyId == currentUser.CompanyId &&
                     (e.Role == UserRole.HR || e.Role == UserRole.HRManager) && !e.IsDeleted, ct);

            await notificationService.SendToManyAsync(
                hrStaff.Select(e => e.Id),
                "New Overtime Request",
                $"{employee.FirstName} {employee.FamilyName} submitted an overtime request for {totalHours:F1} hour(s).",
                NotificationType.OvertimeApplied, overtimeRequest.Id, ct: ct);
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
                    "New Overtime Request",
                    $"{employee.FirstName} {employee.FamilyName} submitted an overtime request for {totalHours:F1} hour(s).",
                    NotificationType.OvertimeApplied, overtimeRequest.Id, ct: ct);
            }
        }

        return Result<ApplyOvertimeResponse>.Success(
            new ApplyOvertimeResponse(overtimeRequest.Id, overtimeRequest.Status.ToString(), totalHours), 201);
    }
}
