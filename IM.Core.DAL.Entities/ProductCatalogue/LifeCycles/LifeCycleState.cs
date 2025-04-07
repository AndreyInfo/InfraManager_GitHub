using Inframanager;
using System;
using System.Collections.Generic;

namespace InfraManager.DAL.ProductCatalogue.LifeCycles;

[ObjectClassMapping(ObjectClass.LifeCycleState)]
[OperationIdMapping(ObjectAction.Insert, OperationID.LifeCycle_Update)]
[OperationIdMapping(ObjectAction.Update, OperationID.LifeCycle_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.LifeCycle_Update)]
[OperationIdMapping(ObjectAction.InsertAs, OperationID.LifeCycle_Update)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.LifeCycle_Properties)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.LifeCycle_Properties)]
public class LifeCycleState
{
    protected LifeCycleState()
    {

    }

    public LifeCycleState(string name, Guid lifeCycleID)
    {
        ID = Guid.NewGuid();
        Name = name;
        LifeCycleID = lifeCycleID;
    }

    public Guid ID { get; init; }
    public string Name { get; init; }
    public Guid LifeCycleID { get; init; }
    public bool IsInRepair { get; init; }
    public bool IsWrittenOff { get; init; }
    public bool IsDefault { get; set; }
    public bool CanIncludeInActiveRequest { get; init; }
    public bool CanIncludeInPurchase { get; init; }
    public bool CanCreateAgreement { get; init; }
    public bool IsApplied { get; init; }
    public bool CanIncludeInInfrastructure { get; init; }


    public virtual LifeCycle LifeCycle { get; }
    public virtual ICollection<LifeCycleStateOperation> LifeCycleStateOperations { get; init; }
}
