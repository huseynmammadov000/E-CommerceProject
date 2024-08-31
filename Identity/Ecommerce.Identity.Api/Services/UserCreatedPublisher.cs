using RabbitMQ.Client;
using System.Text;

namespace Ecommerce.Identity.Api.Services
{
    public class UserCreatedPublisher
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public UserCreatedPublisher()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: "UserCreatedQueue",
                                  durable:false,
                                  exclusive:false,
                                  autoDelete:false,
                                  arguments:null);
        }


        public void PublishUserCreatedEvent(string userId)
        {
            var body = Encoding.UTF8.GetBytes(userId);

            _channel.BasicPublish(exchange:"",
                                  routingKey:"UserCreatedQueue",
                                  basicProperties:null,
                                  body: body);
            Console.WriteLine($"User Id sending {userId}");

        }

        ~UserCreatedPublisher()
        {
            _channel.Close();
            _connection.Close();
        }
    }
}
