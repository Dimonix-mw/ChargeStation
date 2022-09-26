using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace PlatformServiceDAL.Repositories.Interfaces
{
    public interface IRepositoryBase<T> where T : class
    {
        IQueryable<T> QueryAll(Expression<Func<T, bool>> expression = null);
        Task<List<T>> GetList(Expression<Func<T, bool>> expression = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
        Task CreateAsync(T entry);
        Task CreateAsyncRange(List<T> entryList);
        void Update(T entry);
        void UpdateRange(List<T> entryList);
        void Delete(T entry);
        void DeleteRange(List<T> entryList);
        Task<T> FindSingleAsync(Expression<Func<T, bool>> expression,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
        //Task<long> GetSequenceNumber(string sequenceName);

    }
}
