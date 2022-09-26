namespace TelegramBot.Service.Kafka.Common.Publisher
{
    public interface IKafkaService
    {
        void SendMessage(object obj, string topic);

        void SubscribeOnTopic<T>(string topic, Action<T> action, CancellationToken cancellationToken) where T : class;
    }
}
