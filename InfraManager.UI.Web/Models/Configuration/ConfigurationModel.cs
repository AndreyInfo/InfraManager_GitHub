namespace InfraManager.UI.Web.Models.Configuration;

public class ConfigurationModel
{
    public ServiceSettings MailServiceSettings { get; set; }
    public ServiceSettings WorkflowServiceSettings { get; set; }
    public ServiceSettings SearchServiceSettings { get; set; }
    public ServiceSettings ImportServiceSettings { get; set; }
    public ServiceSettings ScheduleServiceSettings { get; set; }
}