namespace InfraManager.DAL.Events
{
    public enum EventSource : int
    {
        Application = 0,
        MailService = 1,
        MonitoringService = 2,
        WorkflowService = 3,
        TelephonyService = 4,
    }
}
