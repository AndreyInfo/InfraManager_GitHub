using System;

namespace InfraManager.WebApi.Contracts.Models.ServiceDesk.WorkOrders
{ 
    public class WorkOrderReferenceListItemModel
    {
        public Guid ID => IMObjID;
        public Guid IMObjID { get; init; }
        public ObjectClass ClassID => ObjectClass.WorkOrder;
        public int Number { get; init; }
        public string ShortDescription { get; init; }
        public DateTime? UtcDatePromised { get; init; }
        public string EntityStateName { get; init; }
        public DateTime UtcDateModified { get; init; }
        public string TypeName { get; init; }
        public Guid? ExecutorID { get; init; }
    }
}
