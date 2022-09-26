using PlatformService.MessageBroker.Common.Models;
using PlatformServiceBLL.DTOs;

namespace PlatformService.MessageBroker.Consumer.Services
{
    public interface IWebApiService
    {
        public Task StartChargeAsync(StartChargeMQRequest request);
    }
}
