using Inframanager;
using System;
using System.Collections.Generic;

namespace InfraManager.DAL.Location;

[ObjectClassMapping(ObjectClass.Room)]
[OperationIdMapping(ObjectAction.Insert, OperationID.Room_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.Room_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.Room_Delete)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.None)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.None)]
public partial class Room : LocationObject, IGloballyIdentifiedEntity, INamedEntity
{ 
    public new int ID { get; }
    
    public string Plan { get; init; }
    public string Note { get; init; }
    public int? TypeID { get; init; }
    public string Size { get; init; }
    public string LocationPoint { get; init; }
    public string ServiceZone { get; init; }
    public string Key { get; init; }
    public int? VisioID { get; init; }
    public string ExternalID { get; init; }
    public Guid? SubdivisionID { get; init; }
    public Guid? PeripheralDatabaseID { get; init; }
    public int? ComplementaryID { get; init; }
    public int? FloorID { get; init; }
    public byte[] RowVersion { get; init; }

    public new Guid IMObjID { get; }
    public virtual Floor Floor { get; init; }
    public virtual RoomType RoomType { get; init; }

    public virtual ICollection<Workplace> Workplaces { get; }

    public string GetName() => Name;

    public override string GetFullPath()
        => Floor is null ? Name : $"{Floor.GetFullPath()}/ {Name}";
}