using HRMS.Application.Common.Models;
using MediatR;

namespace HRMS.Application.Features.Overtime.Commands.ReviewOvertime;

public record ReviewOvertimeCommand(Guid OvertimeRequestId, bool IsApproved, string? Note)
    : IRequest<Result<ReviewOvertimeResponse>>;

public record ReviewOvertimeResponse(Guid OvertimeRequestId, string Status);
