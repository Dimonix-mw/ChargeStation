using PlatformService.MessageBroker.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using PlatformServiceBLL.Services.Interfaces;
using PlatformServiceBLL.DTOs;
using PlatformService.MessageBroker.Publisher;

namespace PlatformService.MessageBroker.Consumer.Services
{
    public class ListenerPaymentServiceRabbitMQ : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly RabbitMQSettings _rabbitMQSettings;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;
        private readonly IRabbitMqService _mqService;

        public ListenerPaymentServiceRabbitMQ(IOptions<RabbitMQSettings> rabbitMQSettings,
            IServiceProvider serviceProvider, ILogger<ListenerPaymentServiceRabbitMQ> logger,
            IRabbitMqService mqService)
        {
            _logger = logger;
            _mqService = mqService;
            _serviceProvider = serviceProvider;
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
                    queue: _rabbitMQSettings.UpdateFillingMQ,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ListenerPaymentServiceRabbitMQ ctor Error");
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
                    var updateRequest = JsonConvert.DeserializeObject<FillingDto>(content);

                    using IServiceScope scope = _serviceProvider.CreateScope();
                    
                    ISessionService sessionService = scope.ServiceProvider.GetRequiredService<ISessionService>();
                    await sessionService.UpdateFillingAsync(updateRequest);
                   
                    _channel.BasicAck(ea.DeliveryTag, false);

                    var updateRequestMQStatus = await sessionService.GetUpdateRequestData(updateRequest.Id);
                    updateRequestMQStatus.Status = 2;
                    updateRequestMQStatus.TotalMoneyAmount = updateRequest.TotalMoneyAmount;
                    updateRequestMQStatus.Minutes = updateRequest.Minutes;
                    updateRequestMQStatus.PumpId = updateRequest.PumpId;

                    _mqService.SendMessage(updateRequestMQStatus, _rabbitMQSettings.UpdateRequestMQ);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"ListenerPaymentServiceRabbitMQ ExecuteAsync Error");
                } 
            };
            try
            {
                _channel.BasicConsume(_rabbitMQSettings.UpdateFillingMQ, false, consumer);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"UpdateFillingMQ BasicConsume Error");
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
