using AutoMapper;
using PlatformService.MessageBroker.Common.Models;
using PlatformService.Utility.Exceptions;
using PlatformServiceBLL.DTOs;
using PlatformServiceBLL.Services.Interfaces;
using PlatformServiceDAL.Entities;
using PlatformServiceDAL.Repositories.Interfaces;

namespace PlatformServiceBLL.Services.Concrete
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<FillingDto> GetFillingDtoAsync(Guid requestId)
        {
            var session = await _unitOfWork.SessionRepository.FindSingleAsync(x => x.RequestId == requestId);
            if (session == null)
            {
                throw new ApplicationException("Session not found");
            }

            var filling = await _unitOfWork.FillingRepository.FindSingleAsync(x => x.Id == session.FillingId);
            if (filling == null)
            {
                throw new ApplicationException("Filling not found");
            }

            return _mapper.Map<FillingDto>(filling);
        }

        public async Task<StartChargeMQRequest> InsertAsync(StartChargeMQRequest request)
        {
            var existedSession = await _unitOfWork.SessionRepository.FindSingleAsync(s => s.RequestId == request.RequestId);
            if (existedSession != null) throw new InsertDbException("Session already in system");

            var filling = _mapper.Map<Filling>(request);
            var session = _mapper.Map<Session>(request);

            await _unitOfWork.SessionRepository.CreateAsync(session);
            await _unitOfWork.FillingRepository.CreateAsync(filling);

            if (await _unitOfWork.SaveCompletedAsync())
            {
                return request;
            }

            throw new Exception("Insert session and filling error");
        }

        public async Task UpdateFillingAsync(FillingDto request)
        {
            var filling = await _unitOfWork.FillingRepository.FindSingleAsync(s => s.Id == request.Id);

            if (filling == null)
            {
                throw new ApplicationException("Filling not found");
            }
            filling.TotalMoneyAmount = request.TotalMoneyAmount;
            filling.Minutes = request.Minutes;
            _unitOfWork.FillingRepository.Update(filling);

            if (await _unitOfWork.SaveCompletedAsync() != true)
            {
                throw new ApplicationException("Filling update not done");
            }
        }

        public async Task<UpdateRequestMQStatus> GetUpdateRequestData(long fillingId)
        {
            var session = await _unitOfWork.SessionRepository.FindSingleAsync(f => f.FillingId == fillingId);
            if (session == null)
            {
                throw new ApplicationException("Session not found");
            }
            var updateRequestMQStatus = new UpdateRequestMQStatus()
            {
                RequestId = session.RequestId,
                UserId = session.UserId,
            };
            return updateRequestMQStatus;
        }

    }
}
