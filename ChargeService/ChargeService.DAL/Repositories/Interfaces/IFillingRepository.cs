using ChargeService.DAL.Context;
using ChargeService.DAL.Entities;

namespace ChargeService.DAL.Repositories.Interfaces
{
    public interface IFillingRepository : IRepositoryBase<Filling>
    {
        Task<long> GetFillingSequence();
    }    
}
