using PlatformServiceDAL.Context;
using PlatformServiceDAL.Entities;
using PlatformServiceDAL.Repositories.Interfaces;

namespace PlatformServiceDAL.Repositories.Concrete
{
    public class SessionRepository : RepositoryBase<Session>, ISessionRepository
    {
        public SessionRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
