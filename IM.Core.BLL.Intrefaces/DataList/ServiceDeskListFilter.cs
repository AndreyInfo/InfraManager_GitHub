using System;

namespace InfraManager.BLL.DataList
{
    public class ServiceDeskListFilter
    {
        public Guid[] IDList { get; set; }
        public bool WithFinishedWorkflow { get; set; }
        public string AfterModifiedMilliseconds { get; set; }
    }
}
