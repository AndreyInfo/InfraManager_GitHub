using Inframanager;
using InfraManager.DAL.ProductCatalogue;
using InfraManager.DAL.ProductCatalogue.LifeCycles;
using System;

namespace InfraManager.DAL.Asset;

[ObjectClassMapping(ObjectClass.DeviceApplication)]
[OperationIdMapping(ObjectAction.Insert, OperationID.DeviceApplication_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.DeviceApplication_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.DeviceApplication_Delete)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.DeviceApplication_Properties)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.DeviceApplication_Properties)]
public class DeviceApplication
{
    public Guid ID { get; init; }
    public string Name { get; set; }
    public string Note { get; set; }
    public Guid DeviceID { get; set; }
    public ObjectClass DeviceClassID { get; set; }
    public Guid? OrganizationItemID { get; set; }
    public ObjectClass? OrganizationItemClassID { get; set; }
    public byte[] RowVersion { get; init; }
    public Guid? PeripheralDatabaseID { get; set; }
    public Guid? ComplementaryID { get; set; }
    public Guid? InfrastructureSegmentID { get; set; }
    public Guid? CriticalityID { get; set; }
    public DateTime? DateAnnuled { get; set; }
    public DateTime? DateReceived { get; set; }
    public Guid? ClientID { get; set; }
    public ObjectClass? ClientClassID { get; set; }
    public Guid? LifeCycleStateID { get; set; }
    public Guid ProductCatalogTypeID { get; set; }
    public virtual ProductCatalogType ProductCatalogType { get; init; }
    public virtual LifeCycleState LifeCycleState { get; init; }
    public virtual Criticality Criticality { get; init; }
    public virtual InfrastructureSegment InfrastructureSegment { get; init; }

}
