using Ecommerce.Identity.Api.Data;
using Ecommerce.Identity.Api.Data.Entities;
using Ecommerce.Identity.Api.Exceptions;
using Ecommerce.Identity.Api.Repostories.Abstactions;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Identity.Api.Repostories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly IdentityDbContext _context;

        public RefreshTokenRepository(IdentityDbContext context)
        {
            _context = context;
           
        }

        public async Task<RefreshToken> GetByUserIdAsync(string userId)
        {
            try
            {
                var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(r => r.UserId == Guid.Parse(userId));
                if (refreshToken is null)
                {
                    throw new RefreshTokenNotExistException();
                }
                return refreshToken;
            }
            catch (Exception)
            {

                throw new Exception();
            }
            
        }

        public async Task<bool> RemoveAsync(string userId)
        {

            try
            {
                var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(r => r.UserId == Guid.Parse(userId));
                if (refreshToken is null)
                {
                    throw new RefreshTokenNotExistException();
                }

                _context.RefreshTokens.Remove(refreshToken);

                return true;
            }
            catch (Exception)
            {

                throw new Exception();
            }
           
            
        }
    }
}
