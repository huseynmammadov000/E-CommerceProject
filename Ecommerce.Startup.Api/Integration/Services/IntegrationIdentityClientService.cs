using System.Net.Mime;
using System.Text;

using Mapster;
using Ardalis.GuardClauses;
using Newtonsoft.Json;

using Ecommerce.Startup.Api.Integration.Models;
using Ecommerce.Startup.Api.Integration.Options;
using Ecommerce.Startup.Api.Integration.Services.Abstractions;
using Ecommerce.Shared.Models.Responses;


namespace Ecommerce.Startup.Api.Integration.Services;

public class IntegrationIdentityClientService : IIntegrationIdentityClientService
{
    private readonly HttpClient _httpClient;
    private readonly IdentityClientOptions _clientOptions;
    private readonly ILogger<IntegrationIdentityClientService> _logger;


    public IntegrationIdentityClientService(
        HttpClient httpClient,
        IdentityClientOptions clientOptions,
        ILogger<IntegrationIdentityClientService> logger
    )
    {
        _httpClient = Guard.Against.Null(httpClient);
        _clientOptions = Guard.Against.Null(clientOptions);
        _logger = Guard.Against.Null(logger);
        _logger.LogInformation("Identity Client Service created.");
    }


    public async Task<IntegrationGetUserIdResponse> GetCurrentUserIdAsync()
    {
        var requestBody = new StringContent(
            string.Empty,
            Encoding.UTF8,
            MediaTypeNames.Application.Json
        );

        using HttpResponseMessage httpResponseMessage = await _httpClient.PostAsync("/api/Auth/get-current-user-id", requestBody);

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