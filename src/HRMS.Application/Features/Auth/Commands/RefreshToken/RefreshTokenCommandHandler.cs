using HRMS.Application.Common.Exceptions;
using HRMS.Application.Common.Models;
using HRMS.Application.Features.Auth.Commands.Login;
using HRMS.Domain.Entities;
using HRMS.Domain.Interfaces.Repositories;
using HRMS.Domain.Interfaces.Services;
using MediatR;

namespace HRMS.Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenCommandHandler(
    IRepository<Domain.Entities.RefreshToken> refreshTokenRepo,
    IRepository<Employee> employeeRepo,
    ITokenService tokenService,
    IUnitOfWork unitOfWork) : IRequestHandler<RefreshTokenCommand, Result<LoginResponse>>
{
    public async Task<Result<LoginResponse>> Handle(RefreshTokenCommand request, CancellationToken ct)
    {
        var tokenHash = tokenService.HashToken(request.RefreshToken);

        var stored = await refreshTokenRepo.FirstOrDefaultAsync(
            t => t.TokenHash == tokenHash, ct);

        if (stored is null || !stored.IsActive)
            return Result<LoginResponse>.Failure("Invalid or expired refresh token.", 401);

        var employee = await employeeRepo.FirstOrDefaultAsync(
            e => e.UserId == stored.UserId && !e.IsDeleted, ct)
            ?? throw new NotFoundException("Employee not found for this token.");

        if (!employee.IsActive)
            return Result<LoginResponse>.Failure("Account is inactive.", 401);

        var rawNewRefresh = tokenService.GenerateRefreshToken();
        var newHash = tokenService.HashToken(rawNewRefresh);

        stored.RevokedAt = DateTime.UtcNow;
        stored.RevokedByIp = request.IpAddress;
        stored.ReplacedByTokenHash = newHash;
        refreshTokenRepo.Update(stored);

        var newRefreshToken = new Domain.Entities.RefreshToken
        {
            UserId = stored.UserId,
            TokenHash = newHash,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow,
            CreatedByIp = request.IpAddress
        };

        await refreshTokenRepo.AddAsync(newRefreshToken, ct);
        await unitOfWork.SaveChangesAsync(ct);

        var accessToken = tokenService.GenerateAccessToken(
            stored.UserId, employee.Email, employee.Role.ToString());

        return Result<LoginResponse>.Success(new LoginResponse(
            accessToken,
            rawNewRefresh,
            DateTime.UtcNow.AddMinutes(15),
            employee.Id,
            $"{employee.FirstName} {employee.FamilyName}",
            employee.Email,
            employee.Role.ToString()));
    }
}
