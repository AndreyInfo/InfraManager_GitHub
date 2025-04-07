using System.Linq;
using Inframanager;
using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.ServiceDesk.Calls;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.ServiceDesk.WorkOrders;

internal class WorkOrderIsAvailableToExecutorViaToz :
    IBuildAvailableToExecutorViaToz<User, WorkOrder>,
    IBuildAvailableToExecutorViaToz<Group, WorkOrder>
{
    private readonly CallIsAvailableToExecutorViaToz _callSpecificationBuilder;

    public WorkOrderIsAvailableToExecutorViaToz(CallIsAvailableToExecutorViaToz callSpecificationBuilder)
    {
        _callSpecificationBuilder = callSpecificationBuilder;
    }

    private Specification<TExecutor> BuildSpecification<TExecutor>(WorkOrder filterBy, ObjectClass ownerClassID)
        where TExecutor : IGloballyIdentifiedEntity
    {
        if (filterBy.WorkOrderReference.ID == WorkOrderReference.NullID
            || filterBy.WorkOrderReference.ObjectClassID != ObjectClass.Call)
        {
            return new Specification<TExecutor>(_ => true);
        }

        return _callSpecificationBuilder.BuildSpecification<TExecutor>(filterBy.WorkOrderReference.ObjectID, ownerClassID);
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