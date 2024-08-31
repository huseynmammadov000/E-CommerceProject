using Mapster;
using System.Net.Mime;
using Ardalis.GuardClauses;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

using Ecommerce.Shared.Models.Responses;
using Ecommerce.Identity.Api.Constants;
using Ecommerce.Identity.Api.Models.Requests;
using Ecommerce.Identity.Api.DTOs;
using Ecommerce.Identity.Api.Services.Abstractions;
using Ecommerce.Identity.Api.Models.Responses;


namespace Ecommerce.Identity.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class AuthController(
    CookieOptions cookieOptions,
     IAuthenticationService authenticationService
    ) : ControllerBase
{
    private readonly CookieOptions _cookieOptions = Guard.Against.Null(cookieOptions);
    private readonly IAuthenticationService _authenticationService = Guard.Against.Null(authenticationService);


    [HttpPost("get-current-user-id")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<ApiResponse<GetUserIdResponse>> GetCurrentUserIdAsync()
    {
        var currentSession = HttpContext.Request.Cookies
            .FirstOrDefault(cookie => cookie.Key == Cookies.SESSION_KEY).Value;
        var currentRefreshToken = HttpContext.Request.Cookies
            .FirstOrDefault(cookie => cookie.Key == Cookies.REFRESHTOKEN_KEY).Value;

        string? clientRealIp = Request.Headers.TryGetValue("X-Client-Ip", out StringValues ipAddr)
            ? ipAddr
            : Request.HttpContext.Connection.RemoteIpAddress?.ToString();
        string clientUserAgent = Request.Headers[HeaderNames.UserAgent].ToString();
    
        return new ApiResponse<GetUserIdResponse>
        {
            Result = true,
            Payload = await _authenticationService.GetCurrentUserIdAsync(currentSession, currentRefreshToken,clientUserAgent,clientRealIp)
        };
    }

    [HttpPost("sign-up")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<ApiResponse> SignUpAsync([FromBody] SignUpRequest request)
    {
        SignUpRequestDto requestDto = request.Adapt<SignUpRequestDto>();

        string? clientRealIp = Request.Headers.TryGetValue("X-Client-Ip", out StringValues ipAddr)
            ? ipAddr
            : Request.HttpContext.Connection.RemoteIpAddress?.ToString();

        requestDto.ClientIpAddress = Guard.Against.Null(clientRealIp);
        requestDto.ClientUserAgent = Request.Headers[HeaderNames.UserAgent].ToString();

        CookieDto signInResponse = await _authenticationService.SignUpAsync(requestDto);

        _cookieOptions.Expires = signInResponse.Session.ExpiredAt;
        HttpContext.Response.Cookies.Append(Cookies.SESSION_KEY, (signInResponse.Session.Id).ToString(), _cookieOptions);

        _cookieOptions.Expires = signInResponse.RefreshToken.ExpiredAt;
        HttpContext.Response.Cookies.Append(Cookies.REFRESHTOKEN_KEY, signInResponse.RefreshToken.Id, _cookieOptions);

        return new ApiResponse
        {
            Result = true
        };
    }

    [HttpPost("sign-in")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<ApiResponse> SignInAsync([FromBody] SignInRequest request)
    {
        SignInRequestDto requestDto = request.Adapt<SignInRequestDto>();

        string? clientRealIp = Request.Headers.TryGetValue("X-Client-Ip", out StringValues ipAddr)
            ? ipAddr
            : Request.HttpContext.Connection.RemoteIpAddress?.ToString();

        requestDto.ClientIpAddress = Guard.Against.Null(clientRealIp);
        requestDto.ClientUserAgent = Request.Headers[HeaderNames.UserAgent].ToString();
        CookieDto signInResponse = await _authenticationService.SignInAsync(requestDto);

        _cookieOptions.Expires = signInResponse.Session.ExpiredAt;
        HttpContext.Response.Cookies.Append(Cookies.SESSION_KEY, (signInResponse.Session.Id.ToString()), _cookieOptions);

        _cookieOptions.Expires = signInResponse.RefreshToken.ExpiredAt;
        HttpContext.Response.Cookies.Append(Cookies.REFRESHTOKEN_KEY, signInResponse.RefreshToken.Id, _cookieOptions);

        return new ApiResponse
        {
            Result = true
        };
    }

    [HttpPost("validate")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<ApiResponse> ValidateAsync()
    {
        var currentSession = HttpContext.Request.Cookies
            .FirstOrDefault(cookie => cookie.Key == Cookies.SESSION_KEY).Value;
        var currentRefreshToken = HttpContext.Request.Cookies
            .FirstOrDefault(cookie => cookie.Key == Cookies.REFRESHTOKEN_KEY).Value;

        string? clientRealIp = Request.Headers.TryGetValue("X-Client-Ip", out StringValues ipAddr)
            ? ipAddr
            : Request.HttpContext.Connection.RemoteIpAddress?.ToString();
        string clientUserAgent = Request.Headers[HeaderNames.UserAgent].ToString();

        var validationResponse = await _authenticationService.ValidateAsync(currentSession, currentRefreshToken, clientUserAgent, Guard.Against.Null(clientRealIp));

        return new ApiResponse
        {
            Result = true,
            Payload = validationResponse
        };
    }

    [HttpPost("refresh")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<ApiResponse> RefreshTokenAsync()
    {
        var currentRefreshToken = HttpContext.Request.Cookies
            .FirstOrDefault(cookie => cookie.Key == Cookies.REFRESHTOKEN_KEY).Value;

        string? clientRealIp = Request.Headers.TryGetValue("X-Client-Ip", out StringValues ipAddr)
            ? ipAddr
            : Request.HttpContext.Connection.RemoteIpAddress?.ToString();
        string clientUserAgent = Request.Headers[HeaderNames.UserAgent].ToString();

        CookieDto cookieDto = await _authenticationService.RefreshAsync(currentRefreshToken, clientUserAgent, Guard.Against.Null(clientRealIp));

        _cookieOptions.Expires = cookieDto.Session.ExpiredAt;
        HttpContext.Response.Cookies.Append(Cookies.SESSION_KEY, (cookieDto.Session.Id).ToString(), _cookieOptions);

        _cookieOptions.Expires = cookieDto.RefreshToken.ExpiredAt;
        HttpContext.Response.Cookies.Append(Cookies.REFRESHTOKEN_KEY, cookieDto.RefreshToken.Id, _cookieOptions);

        return new ApiResponse
        {
            Result = true
        };
    }

    [HttpPost("sign-out")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<ApiResponse> SignOutAsync()
    {
        var currentSession = HttpContext.Request.Cookies.FirstOrDefault(cookie => cookie.Key == Cookies.SESSION_KEY).Value;
        var currentRefreshToken = HttpContext.Request.Cookies.FirstOrDefault(cookie => cookie.Key == Cookies.REFRESHTOKEN_KEY).Value;

        await _authenticationService.SignOutAsync(currentSession, currentRefreshToken);

        HttpContext.Response.Cookies.Delete(Cookies.SESSION_KEY);
        HttpContext.Response.Cookies.Delete(Cookies.REFRESHTOKEN_KEY);

        return new ApiResponse
        {
            Result = true
        };
    }
}