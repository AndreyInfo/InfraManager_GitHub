using InfraManager.BLL.Notification;

namespace InfraManager.BLL.Settings
{
    public class SupportSettingsAgreementsModelDetails
    {
        public bool IsYesCommentNeeded { get; set; }
        public bool IsNoCommentNeeded { get; set; }
        public NotificationNameDetails TemplateStartAgreement { get; set; }
        public NotificationNameDetails TemplateDeleteApprover { get; set; }
    }
}
