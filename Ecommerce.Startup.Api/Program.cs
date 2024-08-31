using CorrelationId.DependencyInjection;
using Ecommerce.Shared.Extensions;
using Ecommerce.Shared.Options;
using Ecommerce.Startup.Api.Context;
using Ecommerce.Startup.Api.Extensions;
using Ecommerce.Startup.Api.Repositories;
using Ecommerce.Startup.Api.Repositories.Abstractions;
using Ecommerce.Startup.Api.Services;
using Ecommerce.Startup.Api.Services.Abstractions;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;


var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.WithOrigins("https://localhost:7195/", "https://localhost:7195/").AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));
//builder.Services.AddHttpClient("YourClientName")
//    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
//    {
//        ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true
//    });
builder.Services.AddControllers();

var dbConfig = builder.Services.AddConfigurationInstance<DbConfig>(config);
builder.Services.AddSqlServerDbContext<StartupDbContext>(dbConfig);

builder.Services.AddScoped<IStartupRepository,StartupRepository>();
builder.Services.AddScoped<IStartupService,StartupService>();
builder.Services.AddScoped<ICategoryRepository,CategoryRepository>();
builder.Services.AddScoped<ICategoryService,CategoryService>();




//builder.Services.AddSingleton<RabbitMQ.Client.IConnectionFactory>(provider =>
//{
//    return new ConnectionFactory
//    {
//        HostName = "localhost",
//        Port = 5672,
//        UserName = "admin",
//        Password = "HM22rt48",
//        VirtualHost = "/",
//        AutomaticRecoveryEnabled = true,
//        NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
//    };
//});
var factory = new ConnectionFactory()
{
    HostName = "localhost", // veya Docker kullanıyorsanız "192.168.99.100" gibi IP adresi
    Port = 5672,            // RabbitMQ'nun AMQP portu
    UserName = "guest",     // RabbitMQ varsayılan kullanıcı adı
    Password = "guest"      // RabbitMQ varsayılan şifre
};

builder.Services.AddSingleton<RabbitMQ.Client.IConnectionFactory>(factory);
//// UserCreatedConsumer'ı Scoped olarak ekleyin
builder.Services.AddScoped<UserCreatedConsumer>();

builder.Services.RegisterHttpClients(config);
builder.Services.AddDefaultCorrelationId();


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

using (var scope = app.Services.CreateScope())
{
    var consumer = scope.ServiceProvider.GetRequiredService<UserCreatedConsumer>();
    consumer.StartListening();
}


//using (var connection = factory.CreateConnection())
//using (var channel = connection.CreateModel())
//{
//    channel.QueueDeclare(queue: "UserCreatedQueue",
//                         durable: false,
//                         exclusive: false,
//                         autoDelete: false,
//                         arguments: null);

//    Console.WriteLine(" [*] Waiting for messages.");

//    var consumer = new EventingBasicConsumer(channel);
//    consumer.Received += (model, ea) =>
//    {
//        var body = ea.Body.ToArray();
//        var message = Encoding.UTF8.GetString(body);
//        Console.WriteLine(" [x] Received {0}", message);
//    };
//    channel.BasicConsume(queue: "UserCreatedQueue",
//                         autoAck: true,
//                         consumer: consumer);

//    Console.WriteLine(" Press [enter] to exit.");
//    Console.ReadLine();
//}

app.MapControllers();

app.UseMiddleware<Ecommerce.Shared.Middlewares.ExceptionHandlerMiddleware>();

app.Run();