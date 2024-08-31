using Ecommerce.Identity.Api.Data.Entities;
using Ecommerce.Identity.Api.Models.Responses;

namespace Ecommerce.Identity.Api.Repostories.Abstactions
{
    public interface ISessionRepository
    {
        Task<Session> GetByUserIdAsync(string userId);

        Task<GetUserIdResponse> GetUserIdBySessionAsync(string session);

        
        Task<bool> RemoveAsync(string userId);
    }
}
