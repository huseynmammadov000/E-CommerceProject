using Ocelot.Configuration.File;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

using Ecommerce.Shared.Extensions;
using Ecommerce.Api.Gateway.Handlers;
using Ecommerce.Api.Gateway.Options;


var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks();


var ocelotConfig = builder.Services.AddConfigurationInstance<OcelotConfig>(config);

builder.Services.AddOcelot().AddDelegatingHandler<DebuggingHandler>(true);

builder.Services.PostConfigure<FileConfiguration>(configuration =>
{
    foreach (var route in ocelotConfig.Routes)
        foreach (var pathTemplate in route.UpstreamPathTemplates)
        {
            var uri = new UriBuilder
            {
                Scheme = route.DownstreamScheme,
                Host = route.DownstreamHostAndPorts[0].Host,
                Port = route.DownstreamHostAndPorts[0].Port,
            }.Uri;

            configuration.Routes.Add(new()
            {
                UpstreamPathTemplate = pathTemplate,
                DownstreamPathTemplate = pathTemplate,
                DownstreamScheme = route.DownstreamScheme,
                DownstreamHostAndPorts =
                [
                    new FileHostAndPort{ Host = uri.Host, Port = uri.Port }
                ]
            });
        }
    foreach (var route in configuration.Routes)
    {
        if (string.IsNullOrWhiteSpace(route.DownstreamScheme))
            route.DownstreamScheme = config.GetValue<string>("Ocelot:DefaultDownstreamScheme");

        if (string.IsNullOrWhiteSpace(route.DownstreamPathTemplate))
            route.DownstreamPathTemplate = route.UpstreamPathTemplate;
    }
});

builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAllOrigins",
            builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
    });


var app = builder.Build();

app.UseCors("AllowAllOrigins");

if (app.Environment.IsDevelopment())
{
    app.Lifetime.ApplicationStopped.Register(
        () => app.Services.GetRequiredService<ILogger<Program>>().LogWarning("Application stopped."));

    app.Lifetime.ApplicationStarted.Register(
        () => app.Services.GetRequiredService<ILogger<Program>>().LogWarning("Application started."));
}

app.UseHealthChecks("/healthcheck");

app.UseWebSockets();
app.UseOcelot().Wait();

app.Run();