using HRMS.Application.Common.Models;
using MediatR;

namespace HRMS.Application.Features.Auth.Commands.Logout;

public record LogoutCommand(string RefreshToken, string? IpAddress = null) : IRequest<Result>;
