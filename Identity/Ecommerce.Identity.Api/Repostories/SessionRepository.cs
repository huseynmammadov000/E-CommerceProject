using Ecommerce.Identity.Api.Data;
using Ecommerce.Identity.Api.Data.Entities;
using Ecommerce.Identity.Api.Exceptions;
using Ecommerce.Identity.Api.Models.Responses;
using Ecommerce.Identity.Api.Repostories.Abstactions;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Identity.Api.Repostories
{
    public class SessionRepository : ISessionRepository
    {
        private readonly IdentityDbContext _context;

        public SessionRepository(IdentityDbContext context)
        {
            _context = context;
        }

        public async Task<Session> GetByUserIdAsync(string Id)
        {
            try
            {
                var session = await _context.Sessions.FirstOrDefaultAsync(r => r.UserId == Guid.Parse(Id));
                if (session is null)
                {

                    throw new SessionNotFoundException();

                }
                return session;
            }
            catch (Exception)
            {

                throw new Exception();
            }

        }


        public async Task<GetUserIdResponse> GetUserIdBySessionAsync(string session)
        {
            var _session = await _context.Sessions.FirstOrDefaultAsync(r => r.Id == session) 
                ?? throw new SessionNotFoundException();

            return new GetUserIdResponse()
            {
                Id = _session.UserId
            };
        }


        public async Task<bool> RemoveAsync(string Id)
        {

            try
            {
                var session = await _context.Sessions.FirstOrDefaultAsync(r => r.UserId == Guid.Parse(Id));
                if (session is null)
                {

                    throw new SessionNotFoundException();

                }

                _context.Sessions.Remove(session);
                return true;
            }
            catch (Exception)
            {

                throw new Exception();
            }
           
        }


    }
}
