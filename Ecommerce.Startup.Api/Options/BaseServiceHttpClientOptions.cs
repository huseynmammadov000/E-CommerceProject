using Ecommerce.Startup.Api.Options.Abstractions;


namespace Ecommerce.Startup.Api.Options;

public class BaseServiceHttpClientOptions : IServiceHttpClientOptions
{
    public string Uri { get; set; }

    public int RetryCount { get; set; }

    public TimeSpan Lifetime { get; set; }
}