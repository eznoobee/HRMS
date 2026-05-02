using HRMS.Application.Features.Auth.Commands.Login;
using HRMS.Application.Features.Auth.Commands.Logout;
using HRMS.Application.Features.Auth.Commands.RefreshToken;
using HRMS.Application.Features.Auth.Commands.Register;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace HRMS.API.Controllers;

public class AuthController : BaseApiController
{
    private string? ClientIp =>
        HttpContext.Connection.RemoteIpAddress?.ToString();

    [Authorize(Roles = "GeneralManager,HRManager")]
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command, CancellationToken ct) =>
        ToResponse(await Sender.Send(command, ct));

    [HttpPost("login")]
    [EnableRateLimiting("auth")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct) =>
        ToResponse(await Sender.Send(new LoginCommand(request.Email, request.Password, ClientIp), ct));

    [HttpPost("refresh")]
    [EnableRateLimiting("auth")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequest request, CancellationToken ct) =>
        ToResponse(await Sender.Send(new RefreshTokenCommand(request.RefreshToken, ClientIp), ct));

    [Authorize]
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Logout([FromBody] LogoutRequest request, CancellationToken ct) =>
        ToResponse(await Sender.Send(new LogoutCommand(request.RefreshToken, ClientIp), ct));

    public record LoginRequest(string Email, string Password);
    public record RefreshRequest(string RefreshToken);
    public record LogoutRequest(string RefreshToken);
}
