using ChargeService.DAL.Context;
using ChargeService.DAL.Entities;
using ChargeService.DAL.Repositories.Interfaces;

namespace ChargeService.DAL.Repositories.Concrete
{
    public class SessionRepository : RepositoryBase<Session>, ISessionRepository
    {
        public SessionRepository(ApplicationDbContext context) : base(context)
        { }

        public async Task<long> GetSessionSequence()
        {
            var sessionSequence = nameof(Session) + "Numbers";
            return await GetSequenceNumber(sessionSequence);
        }
    }
}
