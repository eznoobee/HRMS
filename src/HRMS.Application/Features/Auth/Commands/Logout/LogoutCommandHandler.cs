using HRMS.Application.Common.Models;
using HRMS.Domain.Interfaces.Repositories;
using HRMS.Domain.Interfaces.Services;
using MediatR;

namespace HRMS.Application.Features.Auth.Commands.Logout;

public class LogoutCommandHandler(
    IRepository<Domain.Entities.RefreshToken> refreshTokenRepo,
    ITokenService tokenService,
    IUnitOfWork unitOfWork) : IRequestHandler<LogoutCommand, Result>
{
    public async Task<Result> Handle(LogoutCommand request, CancellationToken ct)
    {
        var tokenHash = tokenService.HashToken(request.RefreshToken);

        var stored = await refreshTokenRepo.FirstOrDefaultAsync(
            t => t.TokenHash == tokenHash, ct);

        if (stored is null || !stored.IsActive)
            return Result.Failure("Token not found or already revoked.", 400);

        stored.RevokedAt = DateTime.UtcNow;
        stored.RevokedByIp = request.IpAddress;
        refreshTokenRepo.Update(stored);
        await unitOfWork.SaveChangesAsync(ct);

        return Result.Success(204);
    }
}
