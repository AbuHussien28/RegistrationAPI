using RegistrationAPI.Core.Models;
using RegistrationAPI.Infrastructure.Repositorys;

namespace RegistrationAPI.Core.Interfaces
{
    public interface IUnitOfWork
    {
        public GenericRepository<Event> Events { get; }
        public RegistrationRepository Registration { get; }
        public GenericRepository<ApplicationUser> Users { get; }
        Task<int> SaveChanges();
    }
}
