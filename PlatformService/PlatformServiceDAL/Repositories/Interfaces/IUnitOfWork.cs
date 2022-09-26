
namespace PlatformServiceDAL.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IFillingRepository FillingRepository { get; }
        ISessionRepository SessionRepository { get; }
        Task<bool> SaveCompletedAsync();
    }

}
