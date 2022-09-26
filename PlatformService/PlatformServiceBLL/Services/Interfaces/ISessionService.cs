using PlatformService.MessageBroker.Common.Models;
using PlatformServiceBLL.DTOs;

namespace PlatformServiceBLL.Services.Interfaces
{
    public interface ISessionService
    {
        Task<StartChargeMQRequest> InsertAsync(StartChargeMQRequest request);
        Task<FillingDto> GetFillingDtoAsync(Guid requestId);
        Task UpdateFillingAsync(FillingDto request);
        Task<UpdateRequestMQStatus> GetUpdateRequestData(long fillingId);
    }
}
