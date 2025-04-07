using Inframanager;

namespace InfraManager.DAL.Asset;


[OperationIdMapping(ObjectAction.Insert, OperationID.SlotType_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.SlotType_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.None)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.SlotType_Properties)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.SlotType_Properties)]
public class SlotType:Catalog<int>
{
    public int? ComplementaryID { get; set; }
}
