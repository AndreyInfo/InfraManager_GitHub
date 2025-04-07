using InfraManager.BLL.Notification;

namespace InfraManager.BLL.Settings;

public class SpecialNotificationOnRequest
{
    public NotificationTemplateName ClientCallRegistrationMessageTemplate { get; init; }
    public NotificationTemplateName CallEmailTemplate { get; init; }
}