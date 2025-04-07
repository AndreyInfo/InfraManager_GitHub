using Inframanager;
using InfraManager.DAL.ServiceDesk.Calls;

namespace InfraManager.DAL.ServiceDesk.WorkOrders;

internal class WorkOrderIsAvailableToExecutorViaSupportLine :
    IBuildAvailableToExecutorViaSupportLine<User, WorkOrder>,
    IBuildAvailableToExecutorViaSupportLine<Group, WorkOrder>
{
    private readonly CallIsAvailableToExecutorViaSupportLine _callSpecificationBuilder;

    public WorkOrderIsAvailableToExecutorViaSupportLine(CallIsAvailableToExecutorViaSupportLine callSpecificationBuilder)
    {
        _callSpecificationBuilder = callSpecificationBuilder;
    }

    private Specification<TExecutor> BuildSpecification<TExecutor>(WorkOrder filterBy, ObjectClass itemClassID)
        where TExecutor : IGloballyIdentifiedEntity
    {
        if (filterBy.WorkOrderReference.ID == WorkOrderReference.NullID
            || filterBy.WorkOrderReference.ObjectClassID != ObjectClass.Call)
        {
            return new Specification<TExecutor>(_ => true);
        }

        return _callSpecificationBuilder.BuildSpecification<TExecutor>(filterBy.WorkOrderReference.ObjectID, itemClassID);
    }

    Specification<User> IBuildSpecification<User, WorkOrder>.Build(WorkOrder filterBy)
    {
        return BuildSpecification<User>(filterBy, ObjectClass.User);
    }

    Specification<Group> IBuildSpecification<Group, WorkOrder>.Build(WorkOrder filterBy)
    {
        return BuildSpecification<Group>(filterBy, ObjectClass.Group);
    }
}