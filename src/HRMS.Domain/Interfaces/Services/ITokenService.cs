namespace HRMS.Domain.Interfaces.Services;

public interface ITokenService
{
    int AccessTokenExpiryMinutes { get; }
    int RefreshTokenExpiryDays { get; }
    string GenerateAccessToken(string userId, string email, string role);
    string GenerateRefreshToken();
    string HashToken(string token);
    string? GetUserIdFromExpiredToken(string token);
}
