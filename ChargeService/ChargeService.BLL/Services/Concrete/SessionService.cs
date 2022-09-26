using AutoMapper;
using ChargeService.BLL.Dtos;
using ChargeService.BLL.Enums;
using ChargeService.BLL.Services.Interfaces;
using ChargeService.DAL.Entities;
using ChargeService.DAL.Repositories.Interfaces;
using ChargeService.MessageBroker.Entities;
using ChargeService.Utility.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ChargeService.BLL.Services.Concrete
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

        public async Task<StartChargeMQRequest> GetStartChargeMQRequestAsync(Guid requestId)
        {
            var session = await _unitOfWork.SessionRepository.FindSingleAsync(s => s.RequestId == requestId,
                i => i.Include(f => f.Filling));
            if (session == null) throw new ApplicationException("Session not found");
            return _mapper.Map<StartChargeMQRequest>(session);
        }

        public async Task<InsertPumpRequestDto> InsertAsync(InsertPumpRequestDto request)
        {
            var existedSession = await _unitOfWork.SessionRepository.FindSingleAsync(s => s.RequestId == request.RequestId);
            if (existedSession != null) throw new ApplicationException("Session already in system");

            var sessionId = await _unitOfWork.SessionRepository.GetSessionSequence();
            var fillingId = await _unitOfWork.FillingRepository.GetFillingSequence();

            var session = _mapper.Map<Session>(request);
            var filling = session.Filling;
            session.Status = (int)StatusEnum.NotStarted; // 0 - Зарядка еще не началась
            session.Created = DateTime.Now.ToUniversalTime();
            session.Id = sessionId;
            session.FillingId = fillingId;
            filling.Id = fillingId;

            await _unitOfWork.FillingRepository.CreateAsync(filling);
            await _unitOfWork.SessionRepository.CreateAsync(session);

            if (await _unitOfWork.SaveCompletedAsync())
            {
                return request;
            }

            throw new ApplicationValidationException("Insert session and filling error");
        }

        public async Task<CheckPumpStatusResponseDto> CheckStatusAsync(CheckPumpStatusRequestDto request)
        {
            var session = await _unitOfWork.SessionRepository.FindSingleAsync(s => s.RequestId == request.RequestId);
            if (session == null) throw new ApplicationException("Session not found");
            return _mapper.Map<CheckPumpStatusResponseDto>(session);
        }

        public async Task UpdateStatusAsync(Guid requestId, int status)
        {
            var session = await _unitOfWork.SessionRepository.FindSingleAsync(s => s.RequestId == requestId);

            if (session == null)
            {
                throw new ApplicationException("Session not found");
            }

            session.Status = status;

            _unitOfWork.SessionRepository.Update(session);

            if (await _unitOfWork.SaveCompletedAsync() != true)
            {
                throw new ApplicationException("Session update not done");
            }
        }
    }
}
