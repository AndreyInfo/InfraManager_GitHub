using InfraManager.BLL.Notification;

namespace InfraManager.BLL.Settings;

public class SpecialNotificationOnReplacement
{
    
    public NotificationTemplateName AddSubstitution { get; init; }
    public NotificationTemplateName DeleteSubstitution { get; init; }
    public NotificationTemplateName ChangeDatesSubstitution { get; init; }
}