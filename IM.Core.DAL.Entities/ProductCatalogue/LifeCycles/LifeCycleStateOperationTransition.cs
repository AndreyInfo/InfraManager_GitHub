using Inframanager;
using System;

namespace InfraManager.DAL.ProductCatalogue.LifeCycles;

//TODO добавить права, ибо в легаси и от бизнеса пока не было четких требований
[OperationIdMapping(ObjectAction.Insert, InfraManager.OperationID.None)]
[OperationIdMapping(ObjectAction.Update, InfraManager.OperationID.None)]
[OperationIdMapping(ObjectAction.Delete, InfraManager.OperationID.None)]
[OperationIdMapping(ObjectAction.InsertAs, InfraManager.OperationID.None)]
[OperationIdMapping(ObjectAction.ViewDetails, InfraManager.OperationID.None)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, InfraManager.OperationID.None)]
public class LifeCycleStateOperationTransition
{
    protected LifeCycleStateOperationTransition()
    {

    }

    public LifeCycleStateOperationTransition(Guid operationID, Guid finishStateID, LifeCycleTransitionMode mode)
    {
        ID = Guid.NewGuid();
        OperationID = operationID;
        FinishStateID = finishStateID;
        Mode = mode;
    }

    public Guid ID { get; init; }

    public Guid OperationID { get; init; }

    public Guid FinishStateID { get; init; }

    public LifeCycleTransitionMode Mode { get; init; }

    public virtual LifeCycleStateOperation Operation { get; init; }
    public virtual LifeCycleState FinishState { get; init; }
}
