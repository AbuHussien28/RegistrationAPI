using System.Linq.Expressions;

namespace RegistrationAPI.Core.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(object id);
        IQueryable<T> GetQueryable();
        //  Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        void AddTODB(T entity);
        //Task AddRangeAsync(IEnumerable<T> entities);
        Task Remove(object id);
        void Update(T entity);
    }
}
