using Inframanager;
using InfraManager.DAL.OrganizationStructure;
using System;
using System.Collections.Generic;
using TimeZone = InfraManager.DAL.ServiceDesk.TimeZone;

namespace InfraManager.DAL.Location;

[ObjectClassMapping(ObjectClass.Building)]
[OperationIdMapping(ObjectAction.Insert, OperationID.Building_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.Building_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.Building_Delete)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.None)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.None)]
public partial class Building : LocationObject, IGloballyIdentifiedEntity, ITimeZoneObject
{
    public new int ID { get; }
    public new string Name { get; init; }
    public string Index { get; init; }
    public string Region { get; init; }
    public string City { get; init; }
    public string Area { get; init; }
    public string Street { get; init; }
    public string HousePart { get; init; }
    public string Housing { get; init; }
    public string WiringScheme { get; init; }
    public string Image { get; init; }
    public string Note { get; init; }
    public int? VisioID { get; init; }
    public string ExternalID { get; init; }
    public Guid? SubdivisionID { get; init; }
    public Guid? OrganizationID { get; init; }
    public string House { get; init; }
    public Guid? PeripheralDatabaseID { get; init; }
    public int? ComplementaryID { get; init; }
    public string TimeZoneID { get; set; }
    public byte[] RowVersion { get; init; }

    public new Guid IMObjID { get; }
    public virtual Organization Organization { get; init; }
    public virtual ICollection<Floor> Floors { get; init; }
    public virtual TimeZone TimeZone { get; init; }

    public override string GetFullPath()
        => Organization is null ? Name : $"{Organization.Name}/ {Name}";
}