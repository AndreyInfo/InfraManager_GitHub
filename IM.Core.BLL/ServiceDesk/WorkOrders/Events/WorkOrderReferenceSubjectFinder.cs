using Inframanager.BLL.Events;
using InfraManager.DAL;
using InfraManager.DAL.ChangeTracking;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.WorkOrders;

namespace InfraManager.BLL.ServiceDesk.WorkOrders.Events
{
    internal class WorkOrderReferenceSubjectFinder : ISubjectFinder<WorkOrder, WorkOrderReference>
    {
        private readonly IFinder<WorkOrderReference> _referenceFinder;

        public WorkOrderReferenceSubjectFinder(IFinder<WorkOrderReference> referenceFinder)
        {
            _referenceFinder = referenceFinder;
        }

        public WorkOrderReference Find(WorkOrder entity, IEntityState originalState)
        {
            var subjectReferenceID =
                entity.WorkOrderReferenceID == WorkOrderReference.NullID
                    ? (long)originalState[nameof(WorkOrder.WorkOrderReferenceID)]
                    : entity.WorkOrderReferenceID;
            return _referenceFinder.Find(subjectReferenceID);
        }
    }
}
