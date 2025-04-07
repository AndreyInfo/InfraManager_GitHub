using System;
using InfraManager.DAL.Events;

namespace InfraManager.DAL.ServiceDesk
{
    public class EnvironmentEvent
    {
        public Guid ID { get; set; }

        public DateTime UtcRegisteredAt { get; set; }

        public long Order { get; set; }

        public EventSource Source { get; set; }

        public EventType Type { get; set; }

        public Guid? OwnerID { get; set; }

        public DateTime? UtcOwnedUntil { get; set; }

        public Guid CauserID { get; set; }

        public bool IsProcessed { get; set; }

        public Guid WorkflowSchemeID { get; set; }
    }
}
