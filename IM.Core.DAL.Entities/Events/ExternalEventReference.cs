using System;

namespace InfraManager.DAL.Events
{
    public class ExternalEventReference
    {
        public ExternalEventReference() // TODO: Сделать конструктор закрытым
        {
        }

        public ExternalEventReference(Guid eventId, Guid workflowId)
        {
            ExternalEventId = eventId;
            WorkflowId = workflowId;
        }

        public Guid ExternalEventId { get; init; }
        public Guid WorkflowId { get; init; }
        public int ConsiderationCount { get; set; }
    }
}
