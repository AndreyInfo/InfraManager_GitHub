using InfraManager.BLL.ColumnMapper;
using NotifictionEntity = InfraManager.DAL.Notification.Notification;

namespace InfraManager.BLL.Notification;
public class NotificationColumnSettings : IColumnMapperSetting<NotifictionEntity, NotificationsListItem>, ISelfRegisteredService<IColumnMapperSetting<NotifictionEntity, NotificationsListItem>>
{
    public void Configure(IColumnMapperSettingsBase<NotifictionEntity, NotificationsListItem> configurer)
    {
        configurer.ShouldBe(x => x.ObjectType, x => x.Class.Name);
    }
}
