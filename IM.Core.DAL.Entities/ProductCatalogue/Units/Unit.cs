using System;
using InfraManager;
using InfraManager.DAL;

namespace Inframanager.DAL.ProductCatalogue.Units;

[ObjectClassMapping(ObjectClass.Unit)]
[OperationIdMapping(ObjectAction.Insert, OperationID.Unit_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.Unit_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.Unit_Delete)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.Unit_Properties)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.Unit_Properties)]
public class Unit:IImportEntity
{
    public Guid ID { get; init; }

    public string Name { get; set; }

    public Guid? ComplementaryID { get; init; }

    public string Code { get; init; }

    public string ExternalID { get; set; }
}