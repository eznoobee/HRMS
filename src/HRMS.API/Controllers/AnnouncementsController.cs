using HRMS.Application.Common.Models;
using HRMS.Application.DTOs;
using HRMS.Application.Features.Announcements.Commands.CreateAnnouncement;
using HRMS.Application.Features.Announcements.Queries.GetAnnouncements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;

[Authorize]
public class AnnouncementsController : BaseApiController
{
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<AnnouncementDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default) =>
        ToResponse(await Sender.Send(new GetAnnouncementsQuery(page, pageSize), ct));

    [HttpPost]
    [ProducesResponseType(typeof(CreateAnnouncementResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateAnnouncementCommand command, CancellationToken ct) =>
        ToResponse(await Sender.Send(command, ct));
}
