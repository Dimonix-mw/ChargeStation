using ChargeService.DAL.Context;

namespace ChargeService.DAL.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        ISessionRepository SessionRepository { get; }
        IFillingRepository FillingRepository { get; }
        Task<bool> SaveCompletedAsync();
    }    
}
