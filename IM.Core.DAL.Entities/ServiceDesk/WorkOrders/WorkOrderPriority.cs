using Inframanager;

namespace InfraManager.DAL.ServiceDesk;

[OperationIdMapping(ObjectAction.Insert, OperationID.None)]
[OperationIdMapping(ObjectAction.Update, OperationID.None)]
[OperationIdMapping(ObjectAction.Delete, OperationID.None)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.None)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.None)]
public class WorkOrderPriority : SequencedLookup, IMarkableForDelete, IDefault, IEntityWithColorInt
{
    protected WorkOrderPriority()
    {
    }
    
    public WorkOrderPriority(string name, int sequence) : base(name, sequence)
    {
    }
    
    public int Color { get; init; }
    public bool Default { get; init; }
    public bool Removed { get; private set; }
    public void MarkForDelete()
    {
        Removed = true;
    }
}
