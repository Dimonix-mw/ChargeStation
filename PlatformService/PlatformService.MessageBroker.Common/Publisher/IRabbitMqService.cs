using PlatformService.MessageBroker.Settings;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace PlatformService.MessageBroker.Publisher
{
    public interface IRabbitMqService
    {
        void SendMessage(object obj, string queueName);
        void SendMessage(string message, string queueName);
    }

    public class RabbitMqService : IRabbitMqService
    {
        private readonly RabbitMQSettings _rabbitMQSettings;
        
        public RabbitMqService(IOptions<RabbitMQSettings> rabbitMQSettings)
        {
            _rabbitMQSettings = rabbitMQSettings.Value;
        }

        public void SendMessage(object obj, string queueName)
        {
            var message = JsonSerializer.Serialize(obj);
            SendMessage(message, queueName);
        }

        public void SendMessage(string message, string queueName)
        {
            string host = _rabbitMQSettings.Hostname;
            var factory = new ConnectionFactory() 
            { 
                HostName = host,
                UserName = _rabbitMQSettings.UserName,
                Password = _rabbitMQSettings.Password,
                VirtualHost = _rabbitMQSettings.VirtualHost
            };
            
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName,
                               durable: true,
                               exclusive: false,
                               autoDelete: false,
                               arguments: null);

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                               routingKey: queueName,
                               basicProperties: null,
                               body: body);
            }
        }
    }
}
