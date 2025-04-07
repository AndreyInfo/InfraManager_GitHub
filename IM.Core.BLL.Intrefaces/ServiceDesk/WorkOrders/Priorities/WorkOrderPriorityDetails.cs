using InfraManager.CrossPlatform.WebApi.Contracts.Common.Attributes;
using System;

namespace InfraManager.BLL.ServiceDesk.WorkOrders.Priorities
{
    public class WorkOrderPriorityDetails
    {
        public Guid ID { get; init; }
        
        public string Name { get; init; }
        
        public string Color { get; init; }

        public int Sequence { get; init; }
        
        public bool Default { get; init; }

        public byte[] RowVersion { get; init; }
    }
}
