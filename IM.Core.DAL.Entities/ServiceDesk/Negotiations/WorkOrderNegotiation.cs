using System;

namespace InfraManager.DAL.ServiceDesk.Negotiations
{
    public class WorkOrderNegotiation : Negotiation
    {
        protected WorkOrderNegotiation()
        {
        }

        public WorkOrderNegotiation(Guid workOrderId) : base(workOrderId, ObjectClass.WorkOrder)
        {
        }
    }
}
