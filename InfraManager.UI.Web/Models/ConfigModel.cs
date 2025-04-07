using InfraManager.Web.DTL.Settings;

namespace InfraManager.UI.Web.Models
{
    public sealed class ConfigModel
    {
        public ConfigModel()
        { }

        #region properties
        public CompilationSettings CompilationSettings { get; set; }
        public LoggerSettings LoggerSettings { get; set; }
        public WebSettings WebSettings { get; set; }
        public ResourcesAreaSettings ResourcesAreaSettings { get; set; }

        public WcfClinetSettings MailServiceSettings { get; set; }
        public WcfClinetSettings ScheduleServiceSettings { get; set; }
        public WcfClinetSettings MonitoringServiceSettings { get; set; }
        public WcfClinetSettings SearchServiceSettings { get; set; }
        public WcfClinetSettings WorkflowServiceSettings { get; set; }
        public WcfClinetSettings TelephonyServiceSettings { get; set; }

        public WcfClinetSettings WebMobileSettings { get; set; }

        //TODO
        //public List<BLL.Session> SessionList { get; set; }

        public string WebLog { get; set; }
        public string ServerName { get; set; }
        public string DbName { get; set; }

        #endregion
    }
}
