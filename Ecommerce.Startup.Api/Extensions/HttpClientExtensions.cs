using Polly;
using Polly.Extensions.Http;

using CorrelationId.HttpClient;

using Microsoft.Extensions.Options;

using Ecommerce.Startup.Api.Integration.Options;
using Ecommerce.Startup.Api.Integration.Services.Abstractions;
using Ecommerce.Startup.Api.Integration.Services;
using Ecommerce.Startup.Api.Options.Abstractions;


namespace Ecommerce.Startup.Api.Extensions;

public static class HttpClientExtensions
{
    public static void RegisterHttpClients(this IServiceCollection services, IConfiguration configuration)
    {
        RegisterClient<IdentityClientOptions, IIntegrationIdentityClientService, IntegrationIdentityClientService>(services, configuration);
    }

    private static IAsyncPolicy<HttpResponseMessage> RetryPolicy => HttpPolicyExtensions
        .HandleTransientHttpError()
        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2.0, retryAttempt)));


    private static void RegisterClient<TOptions, TService, TImplementation>(
        IServiceCollection services,
        IConfiguration configuration
    )
        where TOptions : class, IServiceHttpClientOptions, new()
        where TService : class
        where TImplementation : class, TService
    {
        var optionsSection = configuration.GetSection(typeof(TOptions).Name);
        var serviceClientOptions = optionsSection.Get<TOptions>();

        if (serviceClientOptions == null)
            throw new InvalidOperationException($"Configuration for {typeof(TOptions).Name} not found.");

        services.Configure<TOptions>(optionsSection);

        services.AddHttpClient<TService, TImplementation>(httpClient =>
        {
            httpClient.BaseAddress = new Uri(serviceClientOptions.Uri);
        })
        .SetHandlerLifetime(serviceClientOptions.Lifetime)
        .AddPolicyHandler(RetryPolicy)
        .AddCorrelationIdForwarding()
        .SetHandlerLifetime(TimeSpan.FromMinutes(5));

        services.AddSingleton(provider =>
        {
            return provider.GetRequiredService<IOptions<TOptions>>().Value;
        });
    }
}