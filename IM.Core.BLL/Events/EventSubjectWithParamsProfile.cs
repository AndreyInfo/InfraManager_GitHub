using System.Linq;
using AutoMapper;
using InfraManager.DAL.Events;
using Serilog.Core;

namespace InfraManager.BLL.Events
{
    public class EventSubjectWithParamsProfile : Profile
    {
        public EventSubjectWithParamsProfile()
        {
            CreateMap<Event, EventDetails>()
                .ForMember(x => x.Date, x => x.MapFrom(x => x.Date))
                .ForMember(x => x.UserID, x => x.MapFrom(x => x.UserId))
                .ForMember(x => x.UserName, x => x.MapFrom(x => x.User.FullName))
                .ForMember(x => x.Description, x => x.MapFrom(x => x.Message))
                .ForMember(x => x.ClassID, x =>
                {
                    x.Condition(x => x.EventSubject != null);
                    x.MapFrom(x => x.EventSubject.Count > 0 ? x.EventSubject.First().ClassId : null);
                });
  
            CreateMap<EventSubject, EventSubjectDetails>();
            
            CreateMap<EventSubjectParam, EventSubjectParamDetails>();
        }
    }
}

