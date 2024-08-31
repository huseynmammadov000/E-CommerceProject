using Azure.Core;
using Ecommerce.Identity.Api.Data;
using Ecommerce.Identity.Api.Data.Entities;
using Ecommerce.Identity.Api.DTOs;
using Ecommerce.Identity.Api.Exceptions;
using Ecommerce.Identity.Api.Models.Responses;
using Ecommerce.Identity.Api.Repostories.Abstactions;
using Ecommerce.Identity.Api.Services.Abstractions;
using Ecommerce.Shared.Helpers.Abstractions;
using Mapster;

namespace Ecommerce.Identity.Api.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IHashHelper _hashHelper;
        private readonly IdentityDbContext _identityDbContext;
        private readonly IUsersRepository _userRepository;
        private readonly UserCreatedPublisher _userCreatedPublisher;
        private readonly ISessionRepository _sessionRepository;


        public AuthenticationService(IHashHelper hashHelper, IdentityDbContext identityDbContext, 
            IUsersRepository userRepository, UserCreatedPublisher userCreatedPublisher, ISessionRepository sessionRepository
        )
        {
            _hashHelper = hashHelper;
            _identityDbContext = identityDbContext;
            _userRepository = userRepository;
            _userCreatedPublisher = userCreatedPublisher;
            _sessionRepository = sessionRepository;
        }

        public async Task<GetUserIdResponse> GetCurrentUserIdAsync(string currentSession, string currentRefreshToken, 
            string clientUserAgent, string clientRealIp
        )
        {
            await ValidateAsync(currentSession, currentRefreshToken, clientUserAgent, clientRealIp);

            return await _sessionRepository.GetUserIdBySessionAsync(currentSession);
        }

        public async Task<CookieDto> SignInAsync(SignInRequestDto request)
        {
            UserDto userDto = await _userRepository.GetByUsernameAsync(request.UsernameOrEmail) ?? await _userRepository.GetByEmailAsync(request.UsernameOrEmail);

            var session = new Session()
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userDto.Id,
                ClientRealIp = request.ClientIpAddress,
                ClientUserAgent = request.ClientUserAgent,
                ExpiredAt = DateTime.UtcNow.AddMinutes(30),
            };

            var refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userDto.Id,
                ExpiredAt = DateTime.UtcNow.AddHours(30),
            };


            _identityDbContext.Sessions.Add(session);
            _identityDbContext.RefreshTokens.Add(refreshToken);
            await _identityDbContext.SaveChangesAsync();

            var cookieDto = new CookieDto
            {
                Session = session,
                RefreshToken = refreshToken
            };

            return cookieDto;
        }

        //Burada userRepositoryden istifade edilmeyib ,username ve email yoxlanilmasi kimi seyler userrepositoryde olmalidir .Gelecekde duzeldilmelidir.
        public async Task<CookieDto> SignUpAsync(SignUpRequestDto request)
        {
            var password = await _hashHelper.ToSHA256Async(request.Password);

            var role = _identityDbContext.Roles.FirstOrDefault(r => r.Name == request.RoleName);

            var userRole = new UserRole
            {
                Id = Guid.NewGuid(),
                Role = role,
                RoleId = role.Id,

            };

            List<UserRole> appUserRoles = new();
            appUserRoles.Add(userRole);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Email = request.Email,
                PasswordHash = password.ToString(),
                UserRoles = appUserRoles

            };

            var session = new Session
            {
                Id = Guid.NewGuid().ToString(),
                UserId = user.Id,
                ClientRealIp = request.ClientIpAddress,
                ClientUserAgent = request.ClientUserAgent,
                ExpiredAt = DateTime.UtcNow.AddMinutes(30)
            };


            var refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid().ToString(),
                UserId = user.Id,
                ExpiredAt = DateTime.UtcNow.AddHours(24)

            };


            //_identityDbContext.Users.Add(user);
           var userDto =  await _userRepository.AddAsync(user.Adapt<UserDto>());
            if(userDto == null)
            {
                throw new Exception();
            }
            _identityDbContext.Sessions.Add(session);
            _identityDbContext.RefreshTokens.Add(refreshToken);
            await _identityDbContext.SaveChangesAsync();
            _userCreatedPublisher.PublishUserCreatedEvent(user.Id.ToString());
            var cookieDto = new CookieDto
            {
                Session = session,
                RefreshToken = refreshToken
            };

            return cookieDto;

        }

        public async Task<bool> ValidateAsync(string currentSession, string currentRefreshToken, string clientUserAgent, string clientRealIp)
        {
            //if (!Guid.TryParse(currentSession, out Guid sessionGuid))
            //{
            //    // Eğer currentSession geçerli bir Guid değilse
            //    throw new SessionNotFoundException();

            //}

            // Session'ı veritabanından al
            var validSession = await _identityDbContext.Sessions.FindAsync(currentSession);
            //var validSession = (await _identityDbContext.Sessions.FindAsync(Guid.TryParse(currentSession)).Adapt<SessionDto>();

            if (validSession == null)
            {
                throw new SessionNotFoundException();
            }

            // Session'ı DTO'ya dönüştür
            var validSessionDto = validSession.Adapt<SessionDto>();

            bool isSessionExpired = validSession.ExpiredAt.ToUniversalTime() <= DateTime.UtcNow;

            if (!string.Equals(validSession.ClientRealIp, clientRealIp) || !string.Equals(validSession.ClientUserAgent, clientUserAgent, StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception();
            }

            if (!isSessionExpired)
            {
                throw new Exception();
            }
            return true;
        }

        public async Task<CookieDto> RefreshAsync(string currentRefreshToken, string clientUserAgent, string clientRealIp)
        {
            var token = await _identityDbContext.RefreshTokens.FindAsync(currentRefreshToken);
            if (token == null || token.ExpiredAt <= DateTime.UtcNow)
            {
                throw new RefreshTokenNotExistException();
            }

            var newSession = new Session
            {
                Id = Guid.NewGuid().ToString(),
                UserId = token.UserId,
                ClientRealIp = clientRealIp,
                ClientUserAgent = clientUserAgent,
                ExpiredAt = DateTime.UtcNow.AddMinutes(30)
            };

            var refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid().ToString(),
                UserId = token.UserId,
                ExpiredAt = DateTime.UtcNow.AddHours(24)

            };

            await _identityDbContext.Sessions.AddAsync(newSession);
            await _identityDbContext.RefreshTokens.AddAsync(refreshToken);
            await _identityDbContext.SaveChangesAsync();

            var cookieDto = new CookieDto
            {
                Session = newSession,
                RefreshToken = refreshToken
            };

            return cookieDto;
        }

        public async Task SignOutAsync(string currentSession, string currentRefreshToken)
        {
            if (!Guid.TryParse(currentSession, out Guid sessionGuid))
            {
                // Eğer currentSession geçerli bir Guid değilse
                throw new SessionNotFoundException();

            }

            var validSession = await _identityDbContext.Sessions.FindAsync(sessionGuid);
            var token = await _identityDbContext.RefreshTokens.FindAsync(currentRefreshToken);

            if ( token is null)
            {

               
                throw new RefreshTokenNotExistException();
            }


            if (validSession is not null)
            {
                _identityDbContext.Sessions.Remove(validSession);
            }
            if (token is not null)
            {
                _identityDbContext.RefreshTokens.Remove(token);
            }

            await _identityDbContext.SaveChangesAsync();
        }
    }
}