using System;

namespace InfraManager.BLL.ServiceDesk.Manhours
{
    public class ManhoursWorkDetails
    {
        public Guid ID { get; init; }
        public Guid ObjectID { get; init; }
        public ObjectClass ObjectClassID { get; init; }
        public Guid? ExecutorID { get; init; }
        public string ExecutorFullName { get; init; }
        public Guid? InitiatorID { get; init; }
        public string InitiatorFullName { get; init; }
        public Guid? UserActivityTypeID { get; init; }
        public string UserActivityTypeName { get; init; }
        public int Number { get; init; }
        public string Description { get; init; }
        public ManhoursDetails[] Entries { get; init; }
    }
}
