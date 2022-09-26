namespace ChargeService.MessageBroker.Settings
{
    public class RabbitMQSettings
    {
        public string Hostname { get; set; }
        public string StartChargeMQ { get; set; }
        public string UpdateRequestMQ { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string VirtualHost { get; set; }
        public string Exchange { get; set; }
    }
}
