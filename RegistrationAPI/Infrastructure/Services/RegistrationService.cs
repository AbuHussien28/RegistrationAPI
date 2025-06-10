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
        private readonly IEmailSender emailSender;

        public RegistrationService(IUnitOfWork unitOfWork,IMapper mapper,IEmailSender emailSender)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.emailSender = emailSender;
        }
        //public async Task<string> RegisterToEventAsync(RegistrationCreateDTO dto)
        //{
        //    var existing = await unitOfWork.Registration.GetByUserAndEventAsync(dto.UserId, dto.EventId);
        //    if (existing != null)
        //        return "User is already registered for this event.";
        //    var eventExists = await unitOfWork.Events.GetByIdAsync(dto.EventId);
        //    if (eventExists == null)
        //        return "Event does not exist.";
        //    var registration = mapper.Map<Registration>(dto);

        //    unitOfWork.Registration.AddTODB(registration);
        //    await unitOfWork.SaveChanges();
        //    return "User registered successfully";
        //}
        public async Task<string> RegisterToEventAsync(RegistrationCreateDTO dto)
        {
            var existing = await unitOfWork.Registration.GetByUserAndEventAsync(dto.UserId, dto.EventId);
            if (existing != null)
                return "User is already registered for this event.";

            var eventEntity = await unitOfWork.Events.GetByIdAsync(dto.EventId);
            if (eventEntity == null)
                return "Event does not exist.";

            var user = await unitOfWork.Users.GetByIdAsync(dto.UserId);
            if (user == null)
                return "User does not exist.";

            var registration = mapper.Map<Registration>(dto);
            unitOfWork.Registration.AddTODB(registration);
            await unitOfWork.SaveChanges();
           var emailBody = $@"
        <h2>Hello {user.FullName} </h2>
        <p>You have successfully registered for the event: <strong>{eventEntity.Title}</strong>.</p>
        <p><strong>Date:</strong> {eventEntity.StartDate} to {eventEntity.EndDate}</p>
        <p><strong>Organizer:</strong> {eventEntity.CreatedBy?.UserName ?? "N/A"}</p>
        <p>Thank you for using our system </p>";

            await emailSender.SendEmailAsync(user.Email, "Event Registration Confirmation", emailBody);

            return "User registered successfully and confirmation email sent.";
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
