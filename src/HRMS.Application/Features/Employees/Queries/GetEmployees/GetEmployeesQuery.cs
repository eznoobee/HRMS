using HRMS.Application.Common.Models;
using HRMS.Application.DTOs;
using MediatR;

namespace HRMS.Application.Features.Employees.Queries.GetEmployees;

public record GetEmployeesQuery(
    Guid? DepartmentId = null,
    string? Role = null,
    string? Search = null,
    int Page = 1,
    int PageSize = 20) : IRequest<Result<PagedResult<EmployeeDto>>>;
