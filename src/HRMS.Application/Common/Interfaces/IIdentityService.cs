namespace HRMS.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<(string UserId, string? Error)> CreateUserAsync(string email, string password, string role);
    Task<(string? UserId, string? Error)> SignInAsync(string email, string password);
    Task<bool> UserExistsAsync(string email);
}
