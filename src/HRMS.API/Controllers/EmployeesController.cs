using HRMS.Application.Common.Models;
using HRMS.Application.DTOs;
using HRMS.Application.Features.Employees.Commands.CreateEmployee;
using HRMS.Application.Features.Employees.Commands.DeleteEmployee;
using HRMS.Application.Features.Employees.Commands.UpdateEmployee;
using HRMS.Application.Features.Employees.Queries.GetEmployee;
using HRMS.Application.Features.Employees.Queries.GetEmployees;
using HRMS.Domain.Enums;
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

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeCommand command, CancellationToken ct) =>
        ToResponse(await Sender.Send(command, ct));

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(UpdateEmployeeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateEmployeeRequest request, CancellationToken ct) =>
        ToResponse(await Sender.Send(new UpdateEmployeeCommand(
            id,
            request.FirstName,
            request.FatherName,
            request.GrandfatherName,
            request.FamilyName,
            request.Phone,
            request.AvatarUrl,
            request.DateOfBirth,
            request.JoinDate,
            request.JobTitle,
            request.Role,
            request.DepartmentId,
            request.IsActive), ct));

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(DeleteEmployeeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct) =>
        ToResponse(await Sender.Send(new DeleteEmployeeCommand(id), ct));

    public record UpdateEmployeeRequest(
        string FirstName,
        string FatherName,
        string GrandfatherName,
        string FamilyName,
        string? Phone,
        string? AvatarUrl,
        DateOnly DateOfBirth,
        DateOnly JoinDate,
        string? JobTitle,
        UserRole Role,
        Guid DepartmentId,
        bool IsActive);
}
