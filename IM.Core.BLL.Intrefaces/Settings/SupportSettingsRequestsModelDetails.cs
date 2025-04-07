using InfraManager.BLL.Notification;
using InfraManager.BLL.Workflow;

namespace InfraManager.BLL.Settings;

public class SupportSettingsRequestsModel
{
    public WorkflowSchemeNameDetails Workflow { get; init; }
    public bool UpdateComposition { get; init; }
    public bool HideField { get; init; }
    public CallAddDependencyMode ModeID { get; init; }
    public CallPromiseDateCalculationMode CountTime { get; set; }
    public long Hours { get; init; }
    public long Minutes { get; init; }
    public bool WarnSLA { get; init; }
    public NotificationNameDetails TemplateMail { get; init; }
    public NotificationNameDetails TemplateSignUp { get; init; }
}


