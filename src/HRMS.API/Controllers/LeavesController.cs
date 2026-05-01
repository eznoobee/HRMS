using HRMS.Application.Common.Models;
using HRMS.Application.DTOs;
using HRMS.Application.Features.Leaves.Commands.ApplyLeave;
using HRMS.Application.Features.Leaves.Commands.ReviewLeave;
using HRMS.Application.Features.Leaves.Queries.GetLeaveBalance;
using HRMS.Application.Features.Leaves.Queries.GetLeaveRequests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;

[Authorize]
public class LeavesController : BaseApiController
{
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<LeaveRequestDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] Guid? employeeId,
        [FromQuery] string? status,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default) =>
        ToResponse(await Sender.Send(new GetLeaveRequestsQuery(employeeId, status, page, pageSize), ct));

    [HttpGet("balance")]
    [ProducesResponseType(typeof(IReadOnlyList<LeaveBalanceDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBalance(
        [FromQuery] Guid? employeeId,
        [FromQuery] int? year,
        CancellationToken ct = default) =>
        ToResponse(await Sender.Send(new GetLeaveBalanceQuery(employeeId, year), ct));

    [HttpPost("apply")]
    [ProducesResponseType(typeof(ApplyLeaveResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> Apply([FromBody] ApplyLeaveCommand command, CancellationToken ct) =>
        ToResponse(await Sender.Send(command, ct));

    [HttpPost("{id:guid}/review")]
    [ProducesResponseType(typeof(ReviewLeaveResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Review(Guid id, [FromBody] ReviewLeaveRequest request, CancellationToken ct) =>
        ToResponse(await Sender.Send(new ReviewLeaveCommand(id, request.IsApproved, request.Note), ct));

    public record ReviewLeaveRequest(bool IsApproved, string? Note);
}
