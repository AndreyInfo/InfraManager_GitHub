using System;

namespace InfraManager.BLL.ServiceDesk.WorkOrders
{
    public class WorkOrderTypeData
    {
        public string Name { get; init; }

        public string IconName { get; init; }

        public byte[] RowVersion { get; init; }

        public string WorkflowSchemeIdentifier { get; init; }
        
        public byte TypeClass { get; init; }
        
        public Guid? FormID { get; init; }
        
        public bool Default { get; init; }   
        
        public string Color { get; init; }
    }
}
