using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;
using TelegramBot.Service.Kafka.Common.Settings;

namespace TelegramBot.Service.Kafka.Common.Publisher
{
    public class KafkaService : IKafkaService, IDisposable
    {
        private readonly KafkaSettings _kafkaSettings;
        private IProducer<Null, string> _producer;
        private IConsumer<Ignore, string> _consumer;
        private readonly ProducerConfig _producerConfig;
        private readonly ConsumerConfig _consumerConfig;
        private readonly ILogger<KafkaService> _logger;

        public KafkaService(IOptions<KafkaSettings> kafkaSettings, ILogger<KafkaService> logger)
        {
            _logger = logger;
            _kafkaSettings = kafkaSettings.Value;

            _producerConfig = new ProducerConfig()
            {
                BootstrapServers = _kafkaSettings.Server,
            };

            _producer = new ProducerBuilder<Null, string>(_producerConfig).Build();

            _consumerConfig = new ConsumerConfig()
            {
                BootstrapServers = _kafkaSettings.Server,
                GroupId = "telegramBot-group",
                //EnableAutoCommit = true,
                EnableAutoCommit = false,
                //StatisticsIntervalMs = 5000,
                //SessionTimeoutMs = 6000,
                AutoOffsetReset = AutoOffsetReset.Latest,
                EnablePartitionEof = true,
                PartitionAssignmentStrategy = PartitionAssignmentStrategy.CooperativeSticky
            };
        }

        public void SendMessage(object obj, string topic)
        {
            var message = JsonSerializer.Serialize(obj);

            _producer.ProduceAsync(topic: topic, new Message<Null, string>()
            {
                Value = message
            });
        }

        public void SubscribeOnTopic<T>(string topic, Action<T> action, CancellationToken cancellationToken) where T : class
        {
            const int commitPeriod = 1;
            using (_consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build())
            {
                _consumer.Assign(new List<TopicPartitionOffset> { new TopicPartitionOffset(topic, 0, Offset.Stored) });
                try
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        try
                        {
                            var cr = _consumer.Consume(cancellationToken);
                            if (cr.IsPartitionEOF)
                            {
                                continue;
                            }
                            _logger.LogInformation($"Consumer received message {cr.Message.Value}");
                            action(cr.Message.Value as T);

                            if (cr.Offset % commitPeriod == 0)
                            {
                                try
                                {
                                    _consumer.Commit(cr);
                                }
                                catch (KafkaException ex)
                                {
                                    _logger.LogError(ex, $"Commit error: {ex.Error.Reason}");
                                }
                            }
                        }
                        catch (ConsumeException ex)
                        {
                            _logger.LogError(ex, $"Consume error: {ex.Error.Reason}");
                        }
                    }
                }
                catch (OperationCanceledException ex)
                {
                    _logger.LogError(ex, "Closing consumer.");
                    _consumer.Close();
                }
            }
        }

        public void Dispose()
        {
            _producer?.Dispose();
            _consumer?.Dispose();
        }
    }
}
