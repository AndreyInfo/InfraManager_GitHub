using InfraManager.DAL.ServiceCatalogue;
using System;

namespace InfraManager.DAL.ServiceDesk
{
    public class CallWaitingStamp
    {
        public Guid CallID { get; init; }
        public DateTime UtcDateWaitingStarted { get; set; }
        public DateTime? UtcDateWaitingFinished { get; set; }
    }
}
