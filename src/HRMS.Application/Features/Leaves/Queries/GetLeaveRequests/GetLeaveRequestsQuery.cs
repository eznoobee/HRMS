using HRMS.Application.Common.Models;
using HRMS.Application.DTOs;
using MediatR;

namespace HRMS.Application.Features.Leaves.Queries.GetLeaveRequests;

public record GetLeaveRequestsQuery(
    Guid? EmployeeId = null,
    string? Status = null,
    int Page = 1,
    int PageSize = 20) : IRequest<Result<PagedResult<LeaveRequestDto>>>;
