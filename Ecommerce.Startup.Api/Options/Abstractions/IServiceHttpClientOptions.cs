namespace Ecommerce.Startup.Api.Options.Abstractions;

public interface IServiceHttpClientOptions
{
    public string Uri { get; set; }

    public int RetryCount { get; set; }

    public TimeSpan Lifetime { get; set; }
}