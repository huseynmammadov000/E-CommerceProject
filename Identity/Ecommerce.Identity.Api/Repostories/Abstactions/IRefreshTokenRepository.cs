using Ecommerce.Identity.Api.Data.Entities;

namespace Ecommerce.Identity.Api.Repostories.Abstactions
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken> GetByUserIdAsync(string userId);
        Task<bool> RemoveAsync(string userId);
    }
}
