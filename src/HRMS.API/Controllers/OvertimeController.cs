using HRMS.Application.Common.Models;
using HRMS.Application.DTOs;
using HRMS.Application.Features.Overtime.Commands.ApplyOvertime;
using HRMS.Application.Features.Overtime.Commands.ReviewOvertime;
using HRMS.Application.Features.Overtime.Queries.GetOvertimeRequests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;

[Authorize]
public class OvertimeController : BaseApiController
{
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<OvertimeRequestDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] Guid? employeeId,
        [FromQuery] string? status,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default) =>
        ToResponse(await Sender.Send(new GetOvertimeRequestsQuery(employeeId, status, page, pageSize), ct));

    [HttpPost("apply")]
    [ProducesResponseType(typeof(ApplyOvertimeResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> Apply([FromBody] ApplyOvertimeCommand command, CancellationToken ct) =>
        ToResponse(await Sender.Send(command, ct));

    [HttpPost("{id:guid}/review")]
    [ProducesResponseType(typeof(ReviewOvertimeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Review(Guid id, [FromBody] ReviewOvertimeRequest request, CancellationToken ct) =>
        ToResponse(await Sender.Send(new ReviewOvertimeCommand(id, request.IsApproved, request.Note), ct));

    public record ReviewOvertimeRequest(bool IsApproved, string? Note);
}
