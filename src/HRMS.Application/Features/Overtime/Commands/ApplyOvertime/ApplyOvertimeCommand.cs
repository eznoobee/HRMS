using HRMS.Application.Common.Models;
using MediatR;

namespace HRMS.Application.Features.Overtime.Commands.ApplyOvertime;

public record ApplyOvertimeCommand(
    DateTime StartTime,
    DateTime EndTime,
    string? Reason
) : IRequest<Result<ApplyOvertimeResponse>>;

public record ApplyOvertimeResponse(Guid OvertimeRequestId, string Status, double TotalHours);
