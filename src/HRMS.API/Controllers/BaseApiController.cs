using HRMS.Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status403Forbidden)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public abstract class BaseApiController : ControllerBase
{
    private ISender? _sender;
    protected ISender Sender => _sender ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    protected IActionResult ToResponse<T>(Result<T> result)
    {
        if (result.IsSuccess) return StatusCode(result.StatusCode, result.Data);
        return StatusCode(result.StatusCode, new { error = result.Error });
    }

    protected IActionResult ToResponse(Result result)
    {
        if (result.IsSuccess) return StatusCode(result.StatusCode);
        return StatusCode(result.StatusCode, new { error = result.Error });
    }
}
