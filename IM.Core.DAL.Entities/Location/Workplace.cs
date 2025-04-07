using Inframanager;
using System;

namespace InfraManager.DAL.Location;

[ObjectClassMapping(ObjectClass.Workplace)]
[OperationIdMapping(ObjectAction.Insert, OperationID.Workplace_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.Workplace_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.Workplace_Delete)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.None)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.None)]
public class Workplace : LocationObject, IGloballyIdentifiedEntity, INamedEntity
{
    public new int ID { get; init; }
    
    public string Note { get; init; }
    public string ExternalID { get; init; }
    public Guid? SubdivisionID { get; init; }
    public Guid? PeripheralDatabaseID { get; init; }
    public int? ComplementaryID { get; init; }
    public int? RoomID { get; init; }
    public byte[] RowVersion { get; init; }
    
    public new Guid IMObjID { get; init; }
    public virtual Room Room { get; init; }


    public string GetName() => Name;
    public override string GetFullPath()
        => Room is null ? Name : $"{Room.GetFullPath()}/ {Name}";
}