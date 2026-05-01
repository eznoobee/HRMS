using HRMS.Application.Features.Overtime.Commands.ApplyOvertime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;

[Authorize]
public class OvertimeController : BaseApiController
{
    [HttpPost("apply")]
    [ProducesResponseType(typeof(ApplyOvertimeResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> Apply([FromBody] ApplyOvertimeCommand command, CancellationToken ct) =>
        ToResponse(await Sender.Send(command, ct));
}
