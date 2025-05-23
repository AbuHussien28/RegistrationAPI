using AutoMapper;
using RegistrationAPI.Core.Interfaces;
using RegistrationAPI.Core.Models;
using RegistrationAPI.Infrastructure.UnitOfWorks;
using RegistrationAPI.Shared.DTOS.Registration;

namespace RegistrationAPI.Infrastructure.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public RegistrationService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<string> RegisterToEventAsync(RegistrationCreateDTO dto)
        {
            var existing = await unitOfWork.Registration.GetByUserAndEventAsync(dto.UserId, dto.EventId);
            if (existing != null)
                return "User is already registered for this event.";
            var eventExists = await unitOfWork.Events.GetByIdAsync(dto.EventId);
            if (eventExists == null)
                return "Event does not exist.";
            var registration = mapper.Map<Registration>(dto);

            unitOfWork.Registration.AddTODB(registration);
            await unitOfWork.SaveChanges();
            return "User registered successfully";
        }
        public async Task<List<RegistrationDTO>> GetRegistrationsForEventAsync(int eventId)
        {
            var registrations = await unitOfWork.Registration.GetAllByEventIdAsync(eventId);
            return mapper.Map<List<RegistrationDTO>>(registrations);
        }
        public async Task<List<RegistrationDTO>> GetUserRegistrationsAsync(string userId)
        {
            var registrations = await unitOfWork.Registration.GetAllByUserIdAsync(userId);
            return mapper.Map<List<RegistrationDTO>>(registrations);
        }
    }
   
}
