using HRMS.Application.Common.Models;
using HRMS.Application.DTOs;
using MediatR;

namespace HRMS.Application.Features.Overtime.Queries.GetOvertimeRequests;

public record GetOvertimeRequestsQuery(
    Guid? EmployeeId,
    string? Status,
    int Page,
    int PageSize) : IRequest<Result<PagedResult<OvertimeRequestDto>>>;
