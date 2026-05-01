using HRMS.Application.Common.Models;
using HRMS.Application.DTOs;
using MediatR;

namespace HRMS.Application.Features.Leaves.Queries.GetLeaveBalance;

public record GetLeaveBalanceQuery(Guid? EmployeeId = null, int? Year = null)
    : IRequest<Result<IReadOnlyList<LeaveBalanceDto>>>;
