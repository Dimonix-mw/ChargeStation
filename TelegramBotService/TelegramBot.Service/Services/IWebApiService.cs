using TelegramBot.Service.Entities;
using TelegramBot.Service.Kafka.Common.Entities;

namespace TelegramBot.Service.Services
{
    public interface IWebApiService
    {
        public Task StartChargeAsync(InsertPumpRequest request);
        public Task<string> AutorizationAsync(string login, string password);

        public Task<AnswerRegistration> RegistrationUserAsync(long userId, UserInfo userInfo);
        
    }
}
