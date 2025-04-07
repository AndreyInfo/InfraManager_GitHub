using InfraManager.BLL.Notification;

namespace InfraManager.BLL.Settings;

public class SpecialNotificationOnAgreement
{
    public NotificationTemplateName StartMessageTemplate { get; init; }
    public NotificationTemplateName DeleteMemberMessageTemplate { get; init; }
}