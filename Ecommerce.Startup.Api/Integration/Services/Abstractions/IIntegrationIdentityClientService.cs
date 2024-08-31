using Ecommerce.Startup.Api.Integration.Models;


namespace Ecommerce.Startup.Api.Integration.Services.Abstractions;

public interface IIntegrationIdentityClientService 
{
    Task<IntegrationGetUserIdResponse> GetCurrentUserIdAsync();
}