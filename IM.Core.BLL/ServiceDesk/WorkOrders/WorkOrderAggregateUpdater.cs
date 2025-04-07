using InfraManager.DAL;
using InfraManager.DAL.ChangeTracking;
using InfraManager.DAL.Documents;
using InfraManager.DAL.ServiceDesk;
using System;

namespace InfraManager.BLL.ServiceDesk.WorkOrders
{
    internal class WorkOrderAggregateUpdater : 
        DocumentCountAggregateUpdater<WorkOrder>,
        ISelfRegisteredService<IVisitNewEntity<DocumentReference>>,
        ISelfRegisteredService<IVisitDeletedEntity<DocumentReference>>
    {
        public WorkOrderAggregateUpdater(IFinder<WorkOrder> finder) 
            : base(ObjectClass.WorkOrder, finder.With(x => x.Aggregate))
        {
        }

        protected override void UpdateDocumentCount(WorkOrder entity, Func<int, int> func)
        {
            entity.Aggregate.DocumentCount = func(entity.Aggregate.DocumentCount);
        }
    }
}
