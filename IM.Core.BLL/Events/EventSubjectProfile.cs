using AutoMapper;
using InfraManager.DAL.Events;

namespace InfraManager.BLL.Events
{
    public class EventSubjectProfile : Profile
    {
        public EventSubjectProfile()
        {
            CreateMap<Event, EventDetails>()
                .ForMember(x => x.Description, x => x.MapFrom(x => x.Message))
                .ForMember(x => x.UserName, x => x.MapFrom(x => x.User.FullName))
                .ForMember(x => x.UserID, x => x.MapFrom(x => x.UserId));
        }
    }
}

