using ChargeService.BLL.Dtos;
using ChargeService.BLL.Services.Interfaces;
using ChargeService.Kafka.Common.Publisher;
using ChargeService.Kafka.Common.Settings;
using ChargeService.MessageBroker.Publisher;
using ChargeService.MessageBroker.Settings;
using Microsoft.Extensions.Options;
using System.Text.Json;

public class KafkaConsumerListener : BackgroundService
{
    private readonly KafkaSettings _kafkaSettings;
    private readonly ILogger _logger;
    private readonly IKafkaService _kafkaService;
    private readonly ISessionService _sessionService;
    private readonly RabbitMQSettings _rabbitMQSettings;
    private readonly IRabbitMqService _mqService;
    public KafkaConsumerListener(IOptions<KafkaSettings> kafkaSettings, ILogger<KafkaConsumerListener> loger,
        IKafkaService kafkaService, ISessionService sessionService, IOptions<RabbitMQSettings> rabbitMQSettings,
        IRabbitMqService mqService)
    {
        _kafkaSettings = kafkaSettings.Value;
        _logger = loger;
        _kafkaService = kafkaService;
        _sessionService = sessionService;
        _rabbitMQSettings = rabbitMQSettings.Value;
        _mqService = mqService;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _kafkaService.SubscribeOnTopic<string>(_kafkaSettings.RequestTopic,  msg =>  MessageHandler(msg), stoppingToken);
        return Task.CompletedTask;
    }

    private async Task MessageHandler(string msg)
    {
        var insertPumpRequest = JsonSerializer.Deserialize<InsertPumpRequestDto>(msg);
        if (insertPumpRequest.RequestId == Guid.Empty)
            return;
        _logger.LogInformation($"InsertRequest, RequestId = {insertPumpRequest.RequestId}," +
            $"UserId = {insertPumpRequest.UserId}");
        var responseDto = await _sessionService.InsertAsync(insertPumpRequest);
        var mqMessage = await _sessionService.GetStartChargeMQRequestAsync(insertPumpRequest.RequestId);
        _mqService.SendMessage(mqMessage, _rabbitMQSettings.StartChargeMQ);
    }
}