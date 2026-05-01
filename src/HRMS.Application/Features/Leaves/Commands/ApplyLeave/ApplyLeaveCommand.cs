using HRMS.Application.Common.Models;
using MediatR;

namespace HRMS.Application.Features.Leaves.Commands.ApplyLeave;

public record ApplyLeaveCommand(
    Guid LeaveTypeId,
    DateOnly StartDate,
    DateOnly EndDate,
    string? Reason
) : IRequest<Result<ApplyLeaveResponse>>;

public record ApplyLeaveResponse(Guid LeaveRequestId, string Status, int TotalDays);
