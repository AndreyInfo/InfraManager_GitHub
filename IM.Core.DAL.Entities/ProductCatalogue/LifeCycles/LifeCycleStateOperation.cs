using Inframanager;
using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.ServiceDesk.WorkOrders;
using System;
using System.Collections.Generic;

namespace InfraManager.DAL.ProductCatalogue.LifeCycles;

//TODO добавить права, ибо в легаси и от бизнеса пока не было четких требований
[OperationIdMapping(ObjectAction.Insert, OperationID.None)]
[OperationIdMapping(ObjectAction.Update, OperationID.None)]
[OperationIdMapping(ObjectAction.Delete, OperationID.None)]
[OperationIdMapping(ObjectAction.InsertAs, OperationID.None)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.None)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.None)]
public class LifeCycleStateOperation
{
    protected LifeCycleStateOperation()
    {

    }

    public LifeCycleStateOperation(string name
        , int sequence
        , LifeCycleOperationCommandType commandType
        , Guid lifeCycleStateID)
    {
        ID = Guid.NewGuid();
        Name = name;
        Sequence = sequence;
        CommandType = commandType;
        LifeCycleStateID = lifeCycleStateID;
    }

    public Guid ID { get; init; }
    public string Name { get; init; }
    public int Sequence { get; init; }
    public LifeCycleOperationCommandType CommandType { get; init; }
    public Guid? WorkOrderTemplateID { get; init; }
    public Guid LifeCycleStateID { get; init; }
    public byte[] Icon { get; init; }
    public string IconName { get; init; }

    public virtual WorkOrderTemplate WorkOrderTemplate { get; init; }
    public virtual LifeCycleState LifeCycleState { get; init; }
    public virtual ICollection<RoleLifeCycleStateOperation> RoleLifeCycleStateOperations { get; init; }
    public virtual ICollection<LifeCycleStateOperationTransition> Transitions { get; init; }
}
