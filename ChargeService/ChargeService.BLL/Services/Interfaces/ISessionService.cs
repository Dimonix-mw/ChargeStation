using ChargeService.BLL.Dtos;
using ChargeService.MessageBroker.Entities;

namespace ChargeService.BLL.Services.Interfaces
{
    public interface ISessionService
    {
        Task<StartChargeMQRequest> GetStartChargeMQRequestAsync(Guid requestId);
        Task<InsertPumpRequestDto> InsertAsync(InsertPumpRequestDto request);
        Task<CheckPumpStatusResponseDto> CheckStatusAsync(CheckPumpStatusRequestDto request);
        Task UpdateStatusAsync(Guid requestId, int status);
    }
}
