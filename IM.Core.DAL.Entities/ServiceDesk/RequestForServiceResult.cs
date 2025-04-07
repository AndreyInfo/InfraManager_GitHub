using Inframanager;

namespace InfraManager.DAL.ServiceDesk;

[ObjectClassMapping(ObjectClass.RFCResult)]
[OperationIdMapping(ObjectAction.Insert, OperationID.ChangeRequest_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.ChangeRequest_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.ChangeRequest_Delete)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.ChangeRequest_Properties)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.ChangeRequest_Properties)]
public class RequestForServiceResult : Lookup, IMarkableForDelete
{
    protected RequestForServiceResult() : base()
    {
    }

    public RequestForServiceResult(string name) : base(name)
    {
    }

    public bool Removed { get; private set; }

    public void MarkForDelete()
    {
        Removed = true;
    }
}
