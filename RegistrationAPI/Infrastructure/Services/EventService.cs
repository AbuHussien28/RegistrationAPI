using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using RegistrationAPI.Core.Interfaces;
using RegistrationAPI.Core.Models;
using RegistrationAPI.Infrastructure.UnitOfWorks;
using RegistrationAPI.Shared.DTOS.Events;

namespace RegistrationAPI.Infrastructure.Services
{
    public class EventService : IEventService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public EventService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<EventReadDto> CreateAsync(EventCreateDto dto, string userId)
        {
            var newEvent = mapper.Map<Event>(dto);
            newEvent.CreatedById = userId;
            unitOfWork.Events.AddTODB(newEvent);
            await unitOfWork.SaveChanges();
            return mapper.Map<EventReadDto>(newEvent);
        }

        public async Task<bool> DeleteAsync(int id, string userId, string role)
        {
            var eventToRemove = await unitOfWork.Events.GetByIdAsync(id);
            if (eventToRemove == null) return false;

            if (role == "Organizer" && eventToRemove.CreatedById != userId)
                return false;

            eventToRemove.IsDeleted = true; 
            unitOfWork.Events.Update(eventToRemove); 
            await unitOfWork.SaveChanges();
            return true;
        }

        public async Task<List<EventReadDto>> GetAllAsync()
        {
            var events=await unitOfWork.Events
            .GetQueryable()
            .Where(e => !e.IsDeleted)
            .ToListAsync();
            return mapper.Map<List<EventReadDto>>(events);
        }

        public async Task<EventReadDto> GetByIdAsync(int id)
        {
           var eventById = await unitOfWork.Events.GetByIdAsync(id);
            return mapper.Map<EventReadDto>(eventById);
        }

        public async Task<bool> UpdateAsync(int id, EventCreateDto dto, string userId, string role)
        {
            var existing = await unitOfWork.Events.GetByIdAsync(id);
            if (existing == null) return false;

            if (role == "Organizer" && existing.CreatedById != userId)
                return false;

            mapper.Map(dto, existing); 
            unitOfWork.Events.Update(existing);
            await unitOfWork.SaveChanges();
            return true;
        }
        public async Task<bool> RestoreAsync(int id, string userId, string role)
        {
            var eventToRestore = await unitOfWork.Events.GetByIdAsync(id);
            if (eventToRestore == null || !eventToRestore.IsDeleted) return false;

            if (role == "Organizer" && eventToRestore.CreatedById != userId)
                return false;

            eventToRestore.IsDeleted = false;
            unitOfWork.Events.Update(eventToRestore);
            await unitOfWork.SaveChanges();
            return true;
        }
        public async Task<List<EventReadDto>> GetMyEventsAsync(string userId)
        {
            var events = await unitOfWork.Events
                .GetQueryable()
                .Where(e => e.CreatedById == userId && !e.IsDeleted)
                .ToListAsync();

            return mapper.Map<List<EventReadDto>>(events);
        }
    }
}
