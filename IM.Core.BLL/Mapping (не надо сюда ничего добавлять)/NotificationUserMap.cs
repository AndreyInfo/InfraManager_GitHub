using AutoMapper;
using InfraManager.BLL.Notification;
using InfraManager.DAL.Notification;

namespace InfraManager.BLL.Mapping
{
    public class NotificationUserMap : Profile
    {
        public NotificationUserMap()
        {
            CreateMap<NotificationUser, NotificationUserData>()
                .ReverseMap();
        }
    }
}
