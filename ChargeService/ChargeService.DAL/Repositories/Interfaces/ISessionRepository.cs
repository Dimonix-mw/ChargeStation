using ChargeService.DAL.Entities;

namespace ChargeService.DAL.Repositories.Interfaces
{
    public interface ISessionRepository : IRepositoryBase<Session>
    {
        Task<long> GetSessionSequence(); 
    }   
}
