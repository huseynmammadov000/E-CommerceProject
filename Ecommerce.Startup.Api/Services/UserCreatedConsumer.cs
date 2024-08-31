using Ecommerce.Startup.Api.Context;
using Ecommerce.Startup.Api.Models;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Tasks;


namespace Ecommerce.Startup.Api.Services;

public class UserCreatedConsumer
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    public UserCreatedConsumer(IConnectionFactory connectionFactory, IServiceScopeFactory scopeFactory)
    {
        _serviceScopeFactory = scopeFactory;

        // RabbitMQ bağlantısını ve kanalı başlat
        _connection = connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(queue: "UserCreatedQueue",
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);
    }

    public void StartListening()
    {
        if (_channel == null)
        {
            throw new InvalidOperationException("RabbitMQ channel is not initialized.");
        }

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var userId = Encoding.UTF8.GetString(body);
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<StartupDbContext>();
                await CreateRadarForUser(dbContext, userId);
            }
            // Radar oluşturma işlemi burada yapılacak
            Console.WriteLine($"MEssage {userId}");

        };

        _channel.BasicConsume(queue: "UserCreatedQueue",
                             autoAck: true,
                             consumer: consumer);
    }

    private async Task CreateRadarForUser(StartupDbContext _startupDbContext, string userId)
    {
        var radar = new Radar()
        {
            UserId = Guid.Parse(userId)
        };

        var portfolio = new Portfolio()
        {
            UserId = Guid.Parse(userId)
        };

        await _startupDbContext.Radars.AddAsync(radar);
        await _startupDbContext.Portfolios.AddAsync(portfolio);
        await _startupDbContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
    }
}