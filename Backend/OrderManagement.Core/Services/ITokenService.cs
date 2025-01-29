using Microsoft.AspNetCore.Identity;

namespace OrderManagement.Core.Services
{
    public interface ITokenService
    {
        Task<string> CreateTokenAsync(IdentityUser user, UserManager<IdentityUser> userManager);
    }
}
