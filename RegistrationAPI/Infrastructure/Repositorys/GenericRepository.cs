using Microsoft.EntityFrameworkCore;
using RegistrationAPI.Core.Interfaces;
using RegistrationAPI.Data;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Threading.Tasks;

namespace RegistrationAPI.Infrastructure.Repositorys
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly RegistrationContext context;
        public GenericRepository(RegistrationContext context)
        {
            this.context = context;
        }
        public void AddTODB(T entity)
        {
            context.Set<T>().Add(entity);
        }  
        public async Task<List<T>> GetAllAsync()
        {
            return await context.Set<T>().ToListAsync();
        }
        public async Task<T> GetByIdAsync(object id)
        {
           return await context.Set<T>().FindAsync(id);
        }
        public async Task Remove(object id)
        {
            T entityToRemove = await GetByIdAsync(id);
            context.Set<T>().Remove(entityToRemove);
        }
        public void Update(T entity)
        {
            context.Entry(entity).State = EntityState.Modified;
        }
        public IQueryable<T> GetQueryable()
        {
            return context.Set<T>().AsQueryable();
        }
    }
}
