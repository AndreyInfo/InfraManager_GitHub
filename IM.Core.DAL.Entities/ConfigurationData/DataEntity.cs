using System;
using Inframanager;
using InfraManager.DAL.ProductCatalogue.LifeCycles;
using InfraManager.DAL.ProductCatalogue;

namespace InfraManager.DAL.ConfigurationData;

[ObjectClassMapping(ObjectClass.DataEntity)]
[OperationIdMapping(ObjectAction.Insert, OperationID.DataEntity_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.DataEntity_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.DataEntity_Delete)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.DataEntity_Details)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.DataEntity_Details)]
public class DataEntity
{
    public Guid ID { get; init; }
    public string Name { get; init; }
    public string Note { get; init; }
    public Guid? OrganizationItemID { get; init; }
    public int? OrganizationItemClassID { get; init; }
    public Guid? DeviceApplicationID { get; init; }
    public decimal? Size { get; init; }
    public byte[] RowVersion { get; init; }
    public Guid? PeripheralDatabaseID { get; init; }
    public Guid? ComplementaryID { get; init; }
    public Guid? VolumeID { get; init; }
    public Guid? InfrastructureSegmentID { get; init; }
    public Guid? CriticalityID { get; init; }
    public Guid? ClientID { get; init; }
    public int? ClientClassID { get; init; }
    public DateTime? DateAnnuled { get; init; }
    public DateTime? DateReceived { get; init; }
    public Guid? LifeCycleStateID { get; set; }
    public Guid ProductCatalogTypeID { get; set; }

    public virtual ProductCatalogType Type { get; init; }
    public virtual LifeCycleState LifeCycleState { get; init; }
}