using System.Net.Mime;
using System.Text;

using Mapster;
using Ardalis.GuardClauses;
using Newtonsoft.Json;

using Ecommerce.Startup.Api.Integration.Models;
using Ecommerce.Startup.Api.Integration.Options;
using Ecommerce.Startup.Api.Integration.Services.Abstractions;
using Ecommerce.Shared.Models.Responses;
using Microsoft.Net.Http.Headers;


namespace Ecommerce.Startup.Api.Integration.Services;

public class IntegrationIdentityClientService : IIntegrationIdentityClientService
{
    private readonly HttpClient _httpClient;
    private readonly IdentityClientOptions _clientOptions;
    private readonly ILogger<IntegrationIdentityClientService> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public IntegrationIdentityClientService(
        HttpClient httpClient,
        IdentityClientOptions clientOptions,
        ILogger<IntegrationIdentityClientService> logger,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _httpClient = Guard.Against.Null(httpClient);
        _clientOptions = Guard.Against.Null(clientOptions);
        _logger = Guard.Against.Null(logger);
        _logger.LogInformation("Identity Client Service created.");
        _httpContextAccessor = Guard.Against.Null(httpContextAccessor);
    }


    public async Task<IntegrationGetUserIdResponse> GetCurrentUserIdAsync()
    {
        var requestBody = new StringContent(
            string.Empty,
            Encoding.UTF8,
            MediaTypeNames.Application.Json
        );

        using var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/Auth/get-current-user-id");

        if (_httpContextAccessor.HttpContext.Request.Cookies.Count > 0)
        {
            var cookieHeader = string.Join("; ", _httpContextAccessor.HttpContext.Request.Cookies.Select(c => $"{c.Key}={c.Value}"));
            requestMessage.Headers.Add("Cookie", cookieHeader);
        }

        string? clientRealIp = _httpContextAccessor.HttpContext.Request.Headers.TryGetValue("X-Client-Ip", out var ipAddr)
            ? ipAddr.ToString()
            : _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString();

        string clientUserAgent = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.UserAgent].ToString();

        Guard.Against.Null(clientRealIp);

        requestMessage.Headers.Add("X-Client-Ip", clientRealIp);

        if (!string.IsNullOrEmpty(clientUserAgent))
            requestMessage.Headers.Add("User-Agent", clientUserAgent);

        using var httpResponseMessage = await _httpClient.SendAsync(requestMessage);

        var responseBody = await httpResponseMessage.Content.ReadAsStringAsync();

        if (!this.isJsonContent(httpResponseMessage))
            throw new Exception(JsonConvert.DeserializeObject<string>(responseBody));

        ApiResponse apiResponse = Guard.Against.Null(JsonConvert.DeserializeObject<ApiResponse>(responseBody));

        return apiResponse.Payload.Adapt<IntegrationGetUserIdResponse>();
    }


    private bool isJsonContent(HttpResponseMessage response)
    {
        var contentType = response.Content.Headers.ContentType?.MediaType;
        return contentType != null && contentType.Equals("application/json", StringComparison.OrdinalIgnoreCase);
    }
}