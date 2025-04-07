namespace InfraManager.BLL.AppSettings;

public class SystemSettingsJson
{
    public SystemWebSettings WebSettings { get; init; }

    public string WorkflowServiceBaseURL { get; init; }
    public string SearchServiceBaseURL { get; init; }
    public string MailServiceBaseURL { get; init; }
    public string TelephonyServiceBaseURL { get; init; }
    public string ImportServiceBaseURL { get; init; }
    public string ScheduleServiceBaseURL { get; init; }
}