using System;
using System.Collections.Generic;
using Inframanager;
using InfraManager.DAL.Location;
using InfraManager.DAL.ProductCatalogue;

namespace InfraManager.DAL.Asset;

[ObjectClassMapping(ObjectClass.Rack)]
[OperationIdMapping(ObjectAction.Insert, OperationID.Rack_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.Rack_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.Rack_Delete)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.Rack_Properties)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.Rack_Properties)]
public partial class Rack : 
    Catalog<int>
    , IProduct<CabinetType>
    , ILocationObject
{
    public string FillingScheme { get; init; }
    public string Drawing { get; init; }
    public string Note { get; init; }
    public int? TypeID { get; init; }
    public int? RoomID { get; init; }
    public int? FloorID { get; init; }
    public int BuildingID { get; init; }
    public int Action { get; init; }
    public int? VisioID { get; init; }
    public string ExternalID { get; init; }
    public Guid IMObjID { get; init; }
    public Guid? PeripheralDatabaseID { get; init; }
    public int? ComplementaryID { get; init; }
    public byte NumberingScheme { get; init; }
    public Guid? ComplementaryGuidID { get; init; }

    public virtual Room Room { get; init; }
    public virtual Floor Floor { get; init; }
    public virtual Building Building { get; init; }
    public virtual CabinetType Model { get; init; }
    public virtual ICollection<NetworkDevice> NetworkDevice { get; init; }

    public string GetFullPath()
        => Room is null ? Name : $"{Room.GetFullPath()}/ {Name}";
}
   