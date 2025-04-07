using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Inframanager.BLL.ListView;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.WorkOrders.WorkOrderReferenced;

public class ServiceDeskWorkOrderFilterPredicateBuilder : IBuildListViewFilterPredicates<WorkOrder, WorkOrderListFilter>,
    ISelfRegisteredService<IBuildListViewFilterPredicates<WorkOrder, WorkOrderListFilter>>
{
    public IEnumerable<Expression<Func<WorkOrder, bool>>> Build(Guid userID, WorkOrderListFilter filter)
    {
        var expressions = new List<Expression<Func<WorkOrder, bool>>>();
        if (filter.TypeID.HasValue)
        {
            expressions.Add(x => x.TypeID == filter.TypeID);
        }

        if (filter.InitiatorID.HasValue)
        {
            expressions.Add(x => x.InitiatorID == filter.InitiatorID);
        }

        if (filter.Number.HasValue)
        {
            expressions.Add(x => x.Number == filter.Number);
        }

        if (filter.Ids is { Count: > 0 })
        {
            expressions.Add(x => filter.Ids.Contains(x.IMObjID));
        }

        if (filter.ReferenceID != 0)
        {
            expressions.Add(x => x.WorkOrderReferenceID == filter.ReferenceID);
        }

        if (filter.ShouldSearchFinished.HasValue && !filter.ShouldSearchFinished.Value)
        {
            expressions.Add(x =>
                x.EntityStateID != null || x.WorkflowSchemeID != null || x.WorkflowSchemeVersion == null);
        }

        if (filter.ReferenceIDs != null && filter.ReferenceIDs.Any())
        {
            expressions.Add(x => filter.ReferenceIDs.Contains(x.WorkOrderReferenceID));
        }

        if (!filter.ShouldSearchAccomplished.GetValueOrDefault(true))
        {
            expressions.Add(x => x.UtcDateAccomplished == null || x.UtcDateAssigned == null);
        }

        if (filter.ExecutorID.HasValue)
        {
            expressions.Add(x => x.ExecutorID == filter.ExecutorID);
        }

        if (filter.ReferencedObjectID.HasValue)
        {
            expressions.Add(
                x => x.WorkOrderReference.ObjectID == filter.ReferencedObjectID);
        }

        if (filter.ReferencedObjectClassID.HasValue)
        {
            expressions.Add(
                x => x.WorkOrderReference.ObjectClassID == filter.ReferencedObjectClassID);
        }

        if (filter.UtcDateModified.HasValue)
        {
            expressions.Add(x => x.UtcDateModified >= filter.UtcDateModified);
        }

        return expressions;
    }
}