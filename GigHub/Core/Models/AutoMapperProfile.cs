using AutoMapper;
using GigHub.Core.Dtos;

namespace GigHub.Core.Models
{
    public class AutoMapperProfile : Profile
    {
        public static MapperConfiguration Configure()
        {
            MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ApplicationUser, UserDto>();
                cfg.CreateMap<Gig, GigDto>();
                cfg.CreateMap<Notification, NotificationDto>();
            });

            return mapperConfiguration;
        }
    }


}