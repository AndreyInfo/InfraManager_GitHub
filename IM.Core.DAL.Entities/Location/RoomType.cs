using Inframanager;

namespace InfraManager.DAL.Location;

[OperationIdMapping(ObjectAction.Insert, OperationID.RoomType_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.RoomType_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.None)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.RoomType_Properties)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.RoomType_Properties)]
public class RoomType : Catalog<int>
{
    public int? ComplementaryID { get; set; }
}
