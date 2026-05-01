using HRMS.Application.Common.Models;
using HRMS.Application.DTOs;
using HRMS.Application.Features.Employees.Queries.GetEmployee;
using HRMS.Application.Features.Employees.Queries.GetEmployees;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;

[Authorize]
public class EmployeesController : BaseApiController
{
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<EmployeeDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] Guid? departmentId,
        [FromQuery] string? role,
        [FromQuery] string? search,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default) =>
        ToResponse(await Sender.Send(
            new GetEmployeesQuery(departmentId, role, search, page, pageSize), ct));

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct) =>
        ToResponse(await Sender.Send(new GetEmployeeQuery(id), ct));
}
