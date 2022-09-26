using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using TelegramBot.Service.Entities;
using TelegramBot.Service.Kafka.Common.Entities;

namespace TelegramBot.Service.Services
{
    public class WebApiService : IWebApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public WebApiService(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;

        public async Task<string> AutorizationAsync(string login, string password)
        {
            var client = _httpClientFactory.CreateClient("http-auth");
            var discoveryDocument = await client.GetDiscoveryDocumentAsync();
            var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = discoveryDocument.TokenEndpoint,
                ClientId = "telegramBot",
                ClientSecret = "secret",
                Scope = "telegramBot",
                UserName = login,
                Password = password.ToSha256(),
            });
            
            return tokenResponse.AccessToken;
        }

        public async Task<AnswerRegistration> RegistrationUserAsync(long userId, UserInfo userInfo)
        {
            var userDto = new UserDto()
            {
                Id = userId,
                UserName = userInfo.Login,
                Password = userInfo.Password.ToSha256()
            };
            var client = _httpClientFactory.CreateClient("http-auth");
            var response = await client.PostAsJsonAsync("api/Registration", userDto);
            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                return AnswerRegistration.Created;
            } else if (response.StatusCode == System.Net.HttpStatusCode.Conflict) {
                return AnswerRegistration.Conflict;
            }
            return AnswerRegistration.Error;
        }

        public async Task StartChargeAsync(InsertPumpRequest request)
        {
            var client = _httpClientFactory.CreateClient("http-api");
            var response = await client.PostAsJsonAsync<InsertPumpRequest>("/api/ChargeService", request);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.StatusCode.ToString());
            }
        }
    }

}
