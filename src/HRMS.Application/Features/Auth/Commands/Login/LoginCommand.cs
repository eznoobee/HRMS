using HRMS.Application.Common.Models;
using MediatR;

namespace HRMS.Application.Features.Auth.Commands.Login;

public record LoginCommand(string Email, string Password, string? IpAddress = null) : IRequest<Result<LoginResponse>>;

public record LoginResponse(
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresAt,
    Guid EmployeeId,
    string FullName,
    string Email,
    string Role);
