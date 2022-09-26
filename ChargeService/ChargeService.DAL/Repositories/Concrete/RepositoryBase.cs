using ChargeService.DAL.Context;
using ChargeService.DAL.Repositories.Interfaces;
using ChargeService.Utility.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace ChargeService.DAL.Repositories.Concrete
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _db;
        public RepositoryBase(ApplicationDbContext context)
        {
            _context = context;
            _db = _context.Set<T>();
        }

        public IQueryable<T> QueryAll(Expression<Func<T, bool>> expression = null)
        {
            return expression != null ?
                _context.Set<T>().AsQueryable().Where(expression).AsNoTracking()
                : _context.Set<T>().AsQueryable().AsNoTracking();
        }

        public async Task<List<T>> GetList(Expression<Func<T, bool>> expression = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            IQueryable<T> query = _db;
            if (expression != null)
            {
                query = query.Where(expression);
            }
            if (include != null)
            {
                query = include(query);
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return await query.AsNoTracking().ToListAsync();
        }

        public async Task CreateAsync(T entry)
        {
            await _context.Set<T>().AddAsync(entry);
        }

        public async Task CreateAsyncRange(List<T> entryList)
        {
            await _context.Set<T>().AddRangeAsync(entryList);
        }

        public void Update(T entry)
        {
            _context.Set<T>().Update(entry);
        }

        public void UpdateRange(List<T> entryList)
        {
            _context.Set<T>().UpdateRange(entryList);
        }

        public void Delete(T entry)
        {
            _context.Set<T>().Remove(entry);
        }

        public void DeleteRange(List<T> entryList)
        {
            _context.Set<T>().RemoveRange(entryList);
        }

        public async Task<T> FindSingleAsync(Expression<Func<T, bool>> expression,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            IQueryable<T> query = _db;
            if (include != null)
            {
                query = include(query);
            }
            return await query.FirstOrDefaultAsync(expression);
        }

        public async Task<bool> SaveCompletedAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<long> GetSequenceNumber(string sequenceName)
        {
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = $"select nextval('\"{sequenceName}\"')  as seqNumber";
                _context.Database.OpenConnection();
                using (var result = await command.ExecuteReaderAsync())
                {
                    while (result.Read())
                    {
                        return result.GetInt64(0);
                    }
                }
            }
            throw new ApplicationValidationException($"Error creating sequence {sequenceName}");
        }

    }
}
