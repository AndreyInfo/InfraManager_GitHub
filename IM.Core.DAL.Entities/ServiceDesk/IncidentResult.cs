using Inframanager;

namespace InfraManager.DAL.ServiceDesk;


[ObjectClassMapping(ObjectClass.IncidentResult)]
[OperationIdMapping(ObjectAction.Insert, OperationID.IncidentResult_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.IncidentResult_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.IncidentResult_Delete)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.IncidentResult_Properties)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.IncidentResult_Properties)]
public class IncidentResult : Lookup, IMarkableForDelete
{
    protected IncidentResult()
    {
    }

    public IncidentResult(string name) : base(name)
    {
    }

    public bool Removed { get; private set; }

    public void MarkForDelete()
    {
        Removed = true;
    }
}
