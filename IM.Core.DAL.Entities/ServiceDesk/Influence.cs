using Inframanager;

namespace InfraManager.DAL.ServiceDesk;

/// <summary>
/// Этот класс представляет сущность Влияние
/// </summary>
[ObjectClassMapping(ObjectClass.Influence)]
[OperationIdMapping(ObjectAction.Insert, OperationID.Influence_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.Influence_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.Influence_Delete)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.Influence_Properties)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.Influence_Properties)]
public class Influence : SequencedLookup
{
    protected Influence() : base()
    {
    }

    public Influence(string name, int sequence) : base(name, sequence)
    {
    }
}
