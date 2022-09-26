using PlatformService.MessageBroker.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using PlatformService.MessageBroker.Common.Models;
using PlatformServiceBLL.Services.Interfaces;
using PlatformService.Utility.Exceptions;

namespace PlatformService.MessageBroker.Consumer.Services
{
    public class ListenerChargeServiceRabbitMQ : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly RabbitMQSettings _rabbitMQSettings;
        private readonly IServiceProvider _serviceProvider;
        private readonly IWebApiService _webApiService;
        private readonly ILogger<ListenerChargeServiceRabbitMQ> _logger;
        //private readonly IRabbitMqService _mqService;

        public ListenerChargeServiceRabbitMQ(IOptions<RabbitMQSettings> rabbitMQSettings,
            IServiceProvider serviceProvider, 
            IWebApiService webApiService,
            ILogger<ListenerChargeServiceRabbitMQ> logger)
        {
            _serviceProvider = serviceProvider;
            _webApiService = webApiService;
            _logger = logger;
            _rabbitMQSettings = rabbitMQSettings.Value;
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _rabbitMQSettings.Hostname,
                    UserName = _rabbitMQSettings.UserName,
                    Password = _rabbitMQSettings.Password,
                    VirtualHost = _rabbitMQSettings.VirtualHost
                };
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.QueueDeclare(
                    queue: _rabbitMQSettings.StartChargeMQ,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ListenerChargeServiceRabbitMQ ctor Error");
            }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());

                try
                {
                    var startChargeRequest = JsonConvert.DeserializeObject<StartChargeMQRequest>(content);

                    using IServiceScope scope = _serviceProvider.CreateScope();
                    
                    ISessionService sessionService = scope.ServiceProvider.GetRequiredService<ISessionService>();
                    try
                    {
                        await sessionService.InsertAsync(startChargeRequest);
                    }catch (InsertDbException ex)
                    {
                        _logger.LogError(ex, "Error insert to db");
                    }finally
                    {
                        _channel.BasicAck(ea.DeliveryTag, false);
                    }
                    _logger.LogInformation("Send web api command start charge");
                    await _webApiService.StartChargeAsync(startChargeRequest);

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"ExecuteAsync ListenerChargeServiceRabbitMQ Error");
                } 
            };
            try
            {
                _channel.BasicConsume(_rabbitMQSettings.StartChargeMQ, false, consumer);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"StartChargeMQ BasicConsume Error");
            }
            

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
