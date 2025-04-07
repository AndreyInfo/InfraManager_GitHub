using AutoMapper;
using InfraManager.BLL.Notification;
using InfraManager.DAL.Notification;

namespace InfraManager.BLL.Mapping
{
    public class DefaultNotificationDataMap : Profile
    {
        public DefaultNotificationDataMap()
        {
            CreateMap<DAL.Notification.Notification, DefaultNotificationData>();
        }
    }
}

