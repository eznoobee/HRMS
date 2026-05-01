using HRMS.Application.Common.Interfaces;
using HRMS.Application.Common.Models;
using HRMS.Domain.Entities;
using HRMS.Domain.Interfaces.Repositories;
using HRMS.Domain.Interfaces.Services;
using MediatR;
using RefreshTokenEntity = HRMS.Domain.Entities.RefreshToken;

namespace HRMS.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler(
    IIdentityService identityService,
    IRepository<Employee> employeeRepo,
    IRepository<RefreshTokenEntity> refreshTokenRepo,
    ITokenService tokenService,
    IUnitOfWork unitOfWork) : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken ct)
    {
        var (userId, error) = await identityService.SignInAsync(request.Email, request.Password);
        if (error is not null || userId is null)
            return Result<LoginResponse>.Failure("Invalid email or password.", 401);

        var employee = await employeeRepo.FirstOrDefaultAsync(
            e => e.UserId == userId && !e.IsDeleted, ct);

        if (employee is null || !employee.IsActive)
            return Result<LoginResponse>.Failure("Account is inactive or not found.", 401);

        var accessToken = tokenService.GenerateAccessToken(userId, employee.Email, employee.Role.ToString());
        var rawRefreshToken = tokenService.GenerateRefreshToken();

        var refreshToken = new RefreshTokenEntity
        {
            UserId = userId,
            TokenHash = tokenService.HashToken(rawRefreshToken),
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow,
            CreatedByIp = request.IpAddress
        };

        await refreshTokenRepo.AddAsync(refreshToken, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return Result<LoginResponse>.Success(new LoginResponse(
            accessToken,
            rawRefreshToken,
            DateTime.UtcNow.AddMinutes(15),
            employee.Id,
            $"{employee.FirstName} {employee.LastName}",
            employee.Email,
            employee.Role.ToString()));
    }
}
