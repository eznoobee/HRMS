using HRMS.Application.Common.Models;
using MediatR;

namespace HRMS.Application.Features.Leaves.Commands.ReviewLeave;

public record ReviewLeaveCommand(
    Guid LeaveRequestId,
    bool IsApproved,
    string? Note
) : IRequest<Result<ReviewLeaveResponse>>;

public record ReviewLeaveResponse(Guid LeaveRequestId, string NewStatus);
