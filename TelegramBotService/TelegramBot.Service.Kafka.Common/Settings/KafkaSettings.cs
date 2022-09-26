namespace TelegramBot.Service.Kafka.Common.Settings
{
    public class KafkaSettings
    {
        public string Server { get; set; }
        public string AnswerTopic { get; set; }
        public string RequestTopic { get; set; }
    }
}
