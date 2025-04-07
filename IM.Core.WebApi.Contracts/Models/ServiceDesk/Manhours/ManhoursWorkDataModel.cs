using System;

namespace InfraManager.WebApi.Contracts.Models.ServiceDesk.Manhours
{
    public class ManhoursWorkDataModel
    {
        public Guid? ExecutorID { get; init; }
        public Guid? InitiatorID { get; init; }
        public Guid? UserActivityTypeID { get; init; }
        public string Description { get; init; }
    }
}
