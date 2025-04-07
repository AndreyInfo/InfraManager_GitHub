using System;

namespace InfraManager.DAL.ServiceDesk
{
    public class WorkOrderAggregate
    {
        protected WorkOrderAggregate()
        {
        }

        internal WorkOrderAggregate(Guid workOrderID)
        {
            WorkOrderID = workOrderID;
            DocumentCount = 0;
        }

        public long ID { get; }
        public Guid WorkOrderID { get; }
        public int DocumentCount { get; set; }
    }
}
