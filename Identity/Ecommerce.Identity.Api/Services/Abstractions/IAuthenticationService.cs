using Ecommerce.Identity.Api.DTOs;
using Ecommerce.Identity.Api.Models.Responses;


namespace Ecommerce.Identity.Api.Services.Abstractions
{
    public interface IAuthenticationService
    {
        Task<CookieDto> SignUpAsync(SignUpRequestDto request);
        Task<CookieDto> SignInAsync(SignInRequestDto request);

        Task<GetUserIdResponse> GetCurrentUserIdAsync(
            string currentSession, 
            string currentRefreshToken, 
            string clientUserAgent, 
            string clientRealIp
        );

        Task<bool> ValidateAsync(string currentSession, string currentRefreshToken, string clientUserAgent, string clientRealIp);
        Task<CookieDto> RefreshAsync(string currentRefreshToken, string clientUserAgent, string clientRealIp);
        Task SignOutAsync(string currentSession, string currentRefreshToken);
    }
}
