using ChargeService.DAL.Context;
using ChargeService.DAL.Repositories.Interfaces;

namespace ChargeService.DAL.Repositories.Concrete
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApplicationDbContext _context;
        private bool disposed = false;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        private ISessionRepository _sessionRepository;
        private IFillingRepository _fillingRepository;

        public ISessionRepository SessionRepository =>
            _sessionRepository ??= new SessionRepository(_context);
        public IFillingRepository FillingRepository =>
            _fillingRepository ??= new FillingRepository(_context);

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
