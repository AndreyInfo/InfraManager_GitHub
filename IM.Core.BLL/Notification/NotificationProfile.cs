using AutoMapper;

namespace InfraManager.BLL.Notification
{
    public class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            CreateMap<DAL.Notification.Notification, NotificationDetails>()
                .ForMember(p => p.NotificationRecipient, m => m.MapFrom(scr => scr.NotificationRecipients))
                .ReverseMap();

            CreateMap<DAL.Notification.Notification, NotificationNameDetails>();
        }
    }
}
