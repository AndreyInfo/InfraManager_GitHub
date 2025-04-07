using AutoMapper;
using InfraManager.BLL.Notification;
using InfraManager.DAL.Notification;

namespace InfraManager.BLL.Mapping
{
    public class NotificationRecipientMap : Profile
    {
        public NotificationRecipientMap()
        {
            CreateMap<NotificationRecipient, NotificationRecipientData>()
                .ReverseMap();
        }
    }
}
