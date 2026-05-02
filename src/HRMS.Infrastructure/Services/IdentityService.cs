using HRMS.Application.Common.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace HRMS.Infrastructure.Services;

public class IdentityService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    : IIdentityService
{
    public async Task<(string UserId, string? Error)> CreateUserAsync(string email, string password, string role)
    {
        var user = new IdentityUser { UserName = email, Email = email, EmailConfirmed = true };
        var result = await userManager.CreateAsync(user, password);

        if (!result.Succeeded)
            return (string.Empty, string.Join("; ", result.Errors.Select(e => e.Description)));

        await userManager.AddToRoleAsync(user, role);
        return (user.Id, null);
    }

    public async Task<(string? UserId, string? Error)> SignInAsync(string email, string password)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null) return (null, "Invalid credentials.");

        var result = await signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: true);
        if (!result.Succeeded)
        {
            if (result.IsLockedOut) return (null, "Account is locked. Try again later.");
            return (null, "Invalid credentials.");
        }

        return (user.Id, null);
    }
}
