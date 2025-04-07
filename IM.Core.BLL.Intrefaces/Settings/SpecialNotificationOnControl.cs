using InfraManager.BLL.Notification;

namespace InfraManager.BLL.Settings;

public class SpecialNotificationOnControl
{
    public NotificationTemplateName AddCustomControllers { get; init; }
    
    public NotificationTemplateName DeleteCustomControllers { get; init; }
}