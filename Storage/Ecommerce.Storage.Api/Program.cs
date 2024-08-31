using Ecommerce.Shared.Extensions;
using Ecommerce.Shared.Options;

using Ecommerce.Storage.Api.Data;


var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();


var dbConfig = builder.Services.AddConfigurationInstance<DbConfig>(config);
builder.Services.AddSqlServerDbContext<StorageDbContext>(dbConfig);

builder.Services.AddHealthChecks();

var app = builder.Build();

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