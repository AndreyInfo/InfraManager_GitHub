using System;

namespace InfraManager.BLL.ServiceDesk
{
    public class ServiceDeskListFilter
    {
        public Guid[] IDList { get; set; }
        public bool WithFinishedWorkflow { get; set; }
        public string AfterModifiedMilliseconds { get; set; }
    }
}
