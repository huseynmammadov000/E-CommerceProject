namespace Ecommerce.Api.Gateway.Options;

public class OcelotConfig
{
    public required List<OcelotRouteOptions> Routes { get; set; }

    public required GlobalConfiguration GlobalConfiguration { get; set; }
}

public class OcelotRouteOptions
{
    public required string Name { get; set; }

    public required List<string> UpstreamPathTemplates { get; set; }

    public required string DownstreamPathTemplate { get; set; }

    public required string DownstreamScheme { get; set; }

    public required List<DownstreamHostAndPort> DownstreamHostAndPorts { get; set; }
}

public class GlobalConfiguration
{
    public required string BaseUrl { get; set; }

    public required string RequestIdKey { get; set; }
}

public class DownstreamHostAndPort
{
    public required string Host { get; set; }

    public required int Port { get; set; }
}