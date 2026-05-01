using HRMS.Application.Common.Models;
using HRMS.Application.Features.Auth.Commands.Login;
using MediatR;

namespace HRMS.Application.Features.Auth.Commands.RefreshToken;

public record RefreshTokenCommand(string RefreshToken, string? IpAddress = null)
    : IRequest<Result<LoginResponse>>;
