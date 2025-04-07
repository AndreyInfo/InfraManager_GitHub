using AutoMapper;
using InfraManager.BLL.Notification;
using InfraManager.DAL.Notification;

namespace InfraManager.BLL.Mapping
{
    public class NotificationDataMap : Profile
    {
        public NotificationDataMap()
        {
            CreateMap<DAL.Notification.Notification, NotificationData>();

            CreateMap<DAL.Notification.Notification, NotificationData>()
                .ForMember(p => p.ObjectType, m => m.MapFrom(src => src.Class.Name));
        }
    }
}

