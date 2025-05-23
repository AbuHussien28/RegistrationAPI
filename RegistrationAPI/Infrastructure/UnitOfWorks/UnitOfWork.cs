using RegistrationAPI.Core.Interfaces;
using RegistrationAPI.Core.Models;
using RegistrationAPI.Data;
using RegistrationAPI.Infrastructure.Repositorys;
using Stripe.Tax;
using Registration = RegistrationAPI.Core.Models.Registration;

namespace RegistrationAPI.Infrastructure.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RegistrationContext context;
        private  GenericRepository<Event> events;
        private RegistrationRepository registration;
        private  GenericRepository<ApplicationUser> users;
        public GenericRepository<Event> Events {
            get
            {
                if (events == null)
                     events = new GenericRepository<Event>(context);
                return events;
            }
        }
        public GenericRepository<ApplicationUser> Users
        {
            get
            {
                if (users == null)
                    users = new GenericRepository<ApplicationUser>(context);
                return users;
            }
        }

        public RegistrationRepository Registration
        {
            get
            {
                if (registration == null)
                    registration = new RegistrationRepository(context);
                return registration;
            }
        }

        public UnitOfWork(RegistrationContext context)
        {
            this.context = context;
        }
        public Task<int> SaveChanges()
        {
            return context.SaveChangesAsync();
        }
    }
}
