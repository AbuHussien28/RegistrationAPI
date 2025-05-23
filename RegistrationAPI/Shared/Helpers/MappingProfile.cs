using AutoMapper;
using RegistrationAPI.Core.Models;
using RegistrationAPI.Shared.DTOS;
using RegistrationAPI.Shared.DTOS.Events;
using RegistrationAPI.Shared.DTOS.Registration;

namespace RegistrationAPI.Shared.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterDTO, ApplicationUser>()
           .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))  
           .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
           .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName));
            CreateMap<EventCreateDto, Event>();

            CreateMap<Event, EventReadDto>()
      .ForMember(dest => dest.OrganizerName, opt => opt.MapFrom(src => src.CreatedBy.UserName));

            CreateMap<Registration, RegistrationDTO>()
     .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
     .ForMember(dest => dest.EventName, opt => opt.MapFrom(src => src.Event.Title)).ReverseMap();
            CreateMap<RegistrationCreateDTO, Registration>()
    .ForMember(dest => dest.RegistrationDate, opt => opt.MapFrom(src => DateTime.UtcNow));
        }
    }
}
