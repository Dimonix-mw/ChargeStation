using Microsoft.Extensions.Options;
using System.Text.Json;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Service.Kafka.Common.Entities;
using TelegramBot.Service.Kafka.Common.Publisher;
using TelegramBot.Service.Kafka.Common.Settings;
using TelegramBot.Service.Services;

namespace TelegramBotService
{
    public sealed class TelegramBotListener : BackgroundService
    {
        private ITelegramBotClient _botClient { get; init; }
        private User _user { get; set; }
        private readonly IWebApiService _webApiService;
        private readonly IKafkaService _kafkaService;
        private readonly KafkaSettings _kafkaSettings;
        private readonly ILogger<TelegramBotListener> _logger;

        public TelegramBotListener(IConfiguration configuration, 
            IWebApiService webApiService, 
            IKafkaService kafkaService,
            ILogger<TelegramBotListener> logger,
            IOptions<KafkaSettings> kafkaSettings,
            ITelegramBotClient botClient)
        {
            //var telegram = configuration.GetSection("BotConfiguration"); 
            //var botToken = telegram["BotToken"];

            //_botClient = new TelegramBotClient(botToken);
            _botClient = botClient;
            _webApiService = webApiService;
            _kafkaService = kafkaService;
            _kafkaSettings = kafkaSettings.Value;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            _user = await _botClient.GetMeAsync();

            //Console.Title = _user.Username ?? "TelegramBotService";

            using var cts = new CancellationTokenSource();

            var receiverOptions = new ReceiverOptions()
            {
                AllowedUpdates = Array.Empty<UpdateType>(),
                ThrowPendingUpdates = true,
            };
            //UpdateHandlers.webApiService = _webApiService;
            Task.Run(() => _kafkaService.SubscribeOnTopic<string>(_kafkaSettings.AnswerTopic, msg => SendMessageHandler(msg), stoppingToken));
            UpdateHandlers.kafkaService = _kafkaService;
            UpdateHandlers.kafkaTopicRequest = _kafkaSettings.RequestTopic;
            UpdateHandlers._logger = _logger;
            UpdateHandlers.webApiService = _webApiService;
            _botClient.StartReceiving(updateHandler: UpdateHandlers.HandleUpdateAsync,
                               pollingErrorHandler: UpdateHandlers.PollingErrorHandler,
                               receiverOptions: receiverOptions,
                               cancellationToken: cts.Token);
            _logger.LogInformation($"Start listening for @{_user.Username}");
            //Console.WriteLine($"Start listening for @{_user.Username}");

            // Send cancellation request to stop bot
            //cts.Cancel();
        }

        private async Task SendMessageHandler(string msg)
        {
            var updateRequestMQStatus = JsonSerializer.Deserialize<UpdateRequestMQStatus>(msg);

            await _botClient.SendTextMessageAsync(
                    chatId: updateRequestMQStatus.UserId,
                    text: $"Заправка на станции № {updateRequestMQStatus.PumpId} завершена. Длительность {updateRequestMQStatus.Minutes} мин. " +
                    $"Стоимость {updateRequestMQStatus.TotalMoneyAmount} руб.");
        }
    }
}
