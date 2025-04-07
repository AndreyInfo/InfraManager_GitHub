using Inframanager;
using System;
using System.Collections.Generic;

namespace InfraManager.DAL.Location;

[ObjectClassMapping(ObjectClass.Floor)]
[OperationIdMapping(ObjectAction.Insert, OperationID.Floor_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.Floor_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.Floor_Delete)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.None)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.None)]
public partial class Floor : LocationObject, IGloballyIdentifiedEntity
{
    public new int ID { get; }

    public new string Name { get; init; }
    public string FloorDrawing { get; init; }
    public string Note { get; init; }
    public string MethodNamingRoom { get; init; }
    public int? BuildingID { get; init; }
    public int? VisioID { get; init; }
    public byte[] Vsdfile { get; init; }
    public string ExternalID { get; init; }
    public Guid? SubdivisionID { get; init; }
    public Guid? PeripheralDatabaseID { get; init; }
    public int? ComplementaryID { get; init; }
    public byte[] RowVersion { get; init; }

    public new Guid IMObjID { get; }

    public virtual Building Building { get; init; }
    public virtual ICollection<Room> Rooms { get; init; }

    public override string GetFullPath()
        => Building is null ? Name : $"{Building.GetFullPath()}/ {Name}";
}