using Ardalis.GuardClauses;

using Mapster;


using Ecommerce.Shared.Extensions;
using Ecommerce.Shared.Options;
using Ecommerce.Shared.Helpers.Abstractions;
using Ecommerce.Shared.Helpers;

using Ecommerce.Identity.Api.Data;
using Ecommerce.Identity.Api.Options;
using Ecommerce.Identity.Api.Repostories.Abstactions;
using Ecommerce.Identity.Api.Repostories;
using Ecommerce.Identity.Api.Services.Abstractions;
using Ecommerce.Identity.Api.Services;
using RefreshTokenOptions = Ecommerce.Identity.Api.Options.RefreshTokenOptions;
using SessionOptions = Ecommerce.Identity.Api.Options.SessionOptions;
using CorrelationId.DependencyInjection;



var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.WithOrigins("https://localhost:8080/", "https://localhost:7195/").AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));
builder.Services.AddControllers();

builder.Services.AddSingleton<IHashHelper, HashHelper>();

//builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IUsersService, UsersService>();

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

builder.Services.AddScoped<ISessionRepository,SessionRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<UserCreatedPublisher>();

var dbConfig = builder.Services.AddConfigurationInstance<DbConfig>(config);
builder.Services.AddSqlServerDbContext<IdentityDbContext>(dbConfig);

var sessionOption = builder.Services.AddConfigurationInstance<SessionOptions>(config);
var tokenOption = builder.Services.AddConfigurationInstance<RefreshTokenOptions>(config);

CookieConfig cookieConfig = Guard.Against.Null(config.GetSection(nameof(CookieConfig)).Get<CookieConfig>());
builder.Services.AddSingleton(cookieConfig.Adapt<CookieOptions>());

builder.Services.AddHealthChecks();
builder.Services.AddCorrelationId();
builder.Services.AddHttpLogging(options =>
{
    options.RequestHeaders.Add("x-correlation-id");
    options.ResponseHeaders.Add("x-correlation-id");
});


var app = builder.Build();

// app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.DisplayRequestDuration();
    });

    app.Lifetime.ApplicationStopped.Register(
        () => app.Services.GetRequiredService<ILogger<Program>>().LogWarning("Application stopped."));

    app.Lifetime.ApplicationStarted.Register(
        () => app.Services.GetRequiredService<ILogger<Program>>().LogWarning("Application started."));
}

app.UseHealthChecks("/healthcheck");

app.MapControllers();

app.UseMiddleware<Ecommerce.Shared.Middlewares.ExceptionHandlerMiddleware>();

app.Run();