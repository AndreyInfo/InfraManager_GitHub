using Inframanager;

namespace InfraManager.DAL.ServiceDesk;


[ObjectClassMapping(ObjectClass.ProblemCause)]
[OperationIdMapping(ObjectAction.Insert, OperationID.ProblemCause_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.ProblemCause_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.ProblemCause_Delete)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.ProblemCause_Properties)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.ProblemCause_Properties)]
public class ProblemCause : Lookup
{
    public ProblemCause(string name) : base(name)
    {
    }

    protected ProblemCause()
    {
    }

    public bool Removed { get; set; }
}
