using System;

namespace InfraManager.BLL.ServiceDesk.Manhours
{
    public class ManhoursWorkData
    {
        public Guid ObjectID { get; set; }
        public ObjectClass ObjectClassID { get; set; }
        public Guid? ExecutorID { get; init; }
        public Guid? InitiatorID { get; init; }
        public Guid? UserActivityTypeID { get; init; }
        public string Description { get; init; }
    }
}
