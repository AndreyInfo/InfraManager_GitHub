namespace InfraManager.BLL.AppSettings;

public class SystemSettingData
{
    public SystemWebSettings WebSettings { get; init; }

    public ServiceSettings WorkflowServiceBaseURL { get; init; }
    public ServiceSettings SearchServiceBaseURL { get; init; }
    public ServiceSettings MailServiceBaseURL { get; init; }
    public ServiceSettings TelephonyServiceBaseURL { get; init; }
    public ServiceSettings ImportServiceBaseURL { get; init; }
    public ServiceSettings ScheduleServiceBaseURL { get; init; }
}