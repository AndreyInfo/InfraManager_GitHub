using System;
using Inframanager;

namespace InfraManager.DAL.OrganizationStructure;

[ObjectClassMapping(ObjectClass.JobTitle)]
[OperationIdMapping(ObjectAction.Insert, OperationID.Position_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.Position_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.Position_Delete)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.Position_Properties)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.Position_Properties)]
public class JobTitle : IGloballyIdentifiedEntity
{
    public int ID { get; }
    public string Name { get; set; }
    public int? ComplementaryId { get; set; }
    public Guid IMObjID { get; }
}
