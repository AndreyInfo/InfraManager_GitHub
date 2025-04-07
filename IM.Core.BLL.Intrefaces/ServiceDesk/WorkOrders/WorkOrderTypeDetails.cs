using System;

namespace InfraManager.BLL.ServiceDesk.WorkOrders
{
    public class WorkOrderTypeDetails
    {
        public Guid ID { get; init; }
        public string Name { get; init; }
        public string Color { get; init; }
        public bool Default { get; init; }
        public byte TypeClass { get; init; }
        public string IconName { get; init; }
        public string WorkflowSchemeIdentifier { get; init; }
        public Guid? FormID { get; init; }
        public byte[] RowVersion { get; init; }
    }
}
