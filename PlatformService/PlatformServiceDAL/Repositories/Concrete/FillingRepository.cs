using PlatformServiceDAL.Context;
using PlatformServiceDAL.Entities;
using PlatformServiceDAL.Repositories.Interfaces;

namespace PlatformServiceDAL.Repositories.Concrete
{
    public class FillingRepository : RepositoryBase<Filling>, IFillingRepository
    {
        public FillingRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
