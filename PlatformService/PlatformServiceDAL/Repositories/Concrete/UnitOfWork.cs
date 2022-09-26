using PlatformServiceDAL.Context;
using PlatformServiceDAL.Repositories.Interfaces;

namespace PlatformServiceDAL.Repositories.Concrete
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApplicationDbContext _context;
        private bool disposed = false;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        private IFillingRepository _fillingRepository;
        public IFillingRepository FillingRepository =>
            _fillingRepository ??= new FillingRepository(_context);

        private ISessionRepository _sessionRepository;
        public ISessionRepository SessionRepository =>
            _sessionRepository ??= new SessionRepository(_context);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            this.disposed = true;
        }

        public async Task<bool> SaveCompletedAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

    }
}
