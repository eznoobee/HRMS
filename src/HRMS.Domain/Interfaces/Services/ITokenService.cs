namespace HRMS.Domain.Interfaces.Services;

public interface ITokenService
{
    string GenerateAccessToken(string userId, string email, string role);
    string GenerateRefreshToken();
    string HashToken(string token);
    string? GetUserIdFromExpiredToken(string token);
}
