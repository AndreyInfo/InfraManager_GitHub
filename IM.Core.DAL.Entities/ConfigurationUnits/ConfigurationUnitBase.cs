using Inframanager;
using InfraManager.DAL.Asset;
using System;
using InfraManager.DAL.ProductCatalogue.LifeCycles;
using InfraManager.DAL.ProductCatalogue;

namespace InfraManager.DAL.ConfigurationUnits;

[ObjectClassMapping(ObjectClass.ConfigurationUnitBase)]
[OperationIdMapping(ObjectAction.Insert, OperationID.ConfigurationUnitBase_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.ConfigurationUnitBase_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.ConfigurationUnitBase_Delete)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.ConfigurationUnitBase_Properties)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.ConfigurationUnitBase_Properties)]
public class ConfigurationUnitBase
{
    public ConfigurationUnitBase()
    {
        DateReceived = DateTime.UtcNow;
        Description = string.Empty;
        Note = string.Empty;
        ExternalID = string.Empty;
        Tags = string.Empty;
    }

    public Guid ID { get; init; }
    public int Number { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public string Note { get; init; }
    public string ExternalID { get; init; }
    public string Tags { get; init; }
    public Guid? CreatedBy { get; set; }
    public DateTime? DateReceived { get; init; }
    public Guid ProductCatalogTypeID { get; init; }
    public Guid? LifeCycleStateID { get; init; }
    public Guid? InfrastructureSegmentID { get; init; }
    public Guid? CriticalityID { get; init; }
    public DateTime? DateChanged { get; init; }
    public Guid? ChangedBy { get; init; }
    public DateTime? DateLastInquired { get; init; }
    public DateTime? DateAnnulated { get; init; }
    public Guid? OrganizationItemID { get; init; }
    public int? OrganizationItemClassID { get; init; }
    public Guid? OwnerID { get; init; }
    public Guid? ClientID { get; init; }
    public Guid? ConfigurationUnitSchemeID { get; init; }
    public byte[] RowVersion { get; init; }


    public virtual InfrastructureSegment InfrastructureSegment { get; }
    public virtual Criticality Criticality { get; }
    public virtual ProductCatalogType Type { get; }
    public virtual LifeCycleState LifeCycleState { get; }
}
