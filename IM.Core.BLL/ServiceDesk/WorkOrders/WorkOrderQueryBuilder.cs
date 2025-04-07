using System.Linq;
using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.WorkOrders
{
    internal class WorkOrderQueryBuilder :
        IBuildEntityQuery<WorkOrder, WorkOrderDetails, WorkOrderListFilter>,
        ISelfRegisteredService<IBuildEntityQuery<WorkOrder, WorkOrderDetails, WorkOrderListFilter>>
    {
        private readonly IReadonlyRepository<WorkOrder> _repository;

        public WorkOrderQueryBuilder(
            IReadonlyRepository<WorkOrder> repository)
        {
            _repository = repository;
        }

        public IExecutableQuery<WorkOrder> Query(WorkOrderListFilter filterBy)
        {
            var query = _repository
                .With(x => x.Aggregate)
                .With(x => x.Assignee)
                .With(x => x.BudgetUsage)
                .With(x => x.BudgetUsageCause)
                .With(x => x.Executor)
                .With(x => x.Initiator)
                .With(x => x.Priority)
                .With(x => x.Group)
                .With(x => x.Type)
                .With(x => x.WorkOrderReference)
                .Query();

            if (filterBy.TypeID.HasValue)
            {
                query = query.Where(x => x.TypeID == filterBy.TypeID);
            }

            if (filterBy.InitiatorID.HasValue)
            {
                query = query.Where(x => x.InitiatorID == filterBy.InitiatorID);
            }

            if (filterBy.Number.HasValue)
            {
                query = query.Where(x => x.Number == filterBy.Number);
            }


            if (filterBy.Ids is { Count: > 0 })
            {
                query = query.Where(x => filterBy.Ids.Contains(x.IMObjID));
            }

            if (filterBy.ReferenceID != 0)
            {
                query = query.Where(x => x.WorkOrderReferenceID == filterBy.ReferenceID);
            }

            if (filterBy.ShouldSearchFinished.HasValue && !filterBy.ShouldSearchFinished.Value)
            {
                query = query.Where(x => x.EntityStateID != null || x.WorkflowSchemeID != null || x.WorkflowSchemeVersion == null);
            }

            if (filterBy.ReferenceIDs != null && filterBy.ReferenceIDs.Any())
            {
                query = query.Where(x => filterBy.ReferenceIDs.Contains(x.WorkOrderReferenceID));
            }

            if (!filterBy.ShouldSearchAccomplished.GetValueOrDefault(true))
            {
                query = query.Where(x => x.UtcDateAccomplished == null || x.UtcDateAssigned == null);
            }

            if (filterBy.ExecutorID.HasValue)
            {
                query = query.Where(x => x.ExecutorID == filterBy.ExecutorID);
            }

            if (filterBy.ReferencedObjectID.HasValue)
            {
                query = query.Where(
                    x => x.WorkOrderReference.ObjectID == filterBy.ReferencedObjectID);
            }

            if (filterBy.ReferencedObjectClassID.HasValue)
            {
                query = query.Where(
                    x => x.WorkOrderReference.ObjectClassID == filterBy.ReferencedObjectClassID);
            }
            
            if (filterBy.UtcDateModified.HasValue)
            {
                query = query.Where(x => x.UtcDateModified >= filterBy.UtcDateModified);
            }

            return query;
        }
    }
}
