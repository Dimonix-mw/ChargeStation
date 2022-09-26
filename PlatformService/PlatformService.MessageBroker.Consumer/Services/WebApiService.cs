using AutoMapper;
using PlatformService.MessageBroker.Common.Models;
using PlatformServiceBLL.DTOs;

namespace PlatformService.MessageBroker.Consumer.Services
{
    public class WebApiService : IWebApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMapper _mapper;

        public WebApiService(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;
    
        public async Task StartChargeAsync(StartChargeMQRequest request)
        {
            var client = _httpClientFactory.CreateClient("http-api");
            var response = await client.GetAsync($"Emulator?RequestId={request.RequestId}&&Minutes={request.Minutes}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.StatusCode.ToString());
            }
            //var answer = response.Content.ReadFromJsonAsync<StartChargeMQRequest>();
            
            //return _mapper.Map<CreateFillingDto>(answer);
        }
    }
}
