using Inframanager;

namespace InfraManager.DAL.Asset;

[ObjectClassMapping(ObjectClass.ConnectorType)]
[OperationIdMapping(ObjectAction.Insert, OperationID.ConnectorType_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.ConnectorType_Update)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.None)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.None)]

public class ConnectorType
{
    public int ID { get; init; }
    public string Name { get; init; }
    public int PairCount { get; init; }
    public int MediumID { get; init; }
    public string Image { get; init; }
    public int? ComplementaryID { get; init; }

    public virtual Medium Medium { get; init; }
}
