using System;
using Inframanager;
using InfraManager.DAL.Asset;

namespace InfraManager.DAL.ProductCatalogue;

[ObjectClassMapping(ObjectClass.SoftwareLicenseModel)]
[OperationIdMapping(ObjectAction.Insert, OperationID.SoftwareLicenseModel_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.SoftwareLicenseModel_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.SoftwareLicenseModel_Delete)]
[OperationIdMapping(ObjectAction.ViewDetails,OperationID.SoftwareLicenseModel_Properties)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.None)]
public class SoftwareLicenseModel : IProductModel<Guid>
    , IImportEntity
{
    public Guid IMObjID { get; init; }
    
    public Guid ProductCatalogTypeID { get; init; }

    public Guid SoftwareModelID { get; init; }
    
    public short SoftwareLicenceTypeID { get; init; }
    
    public short SoftwareLicenceSchemeEnum { get; init; }
    
    public int SoftwareExecutionCount { get; init; }
    
    public string Name { get; set; }
    
    public bool Removed { get; set; }
    
    public byte[] RowVersion {get; init; }
    
    public string ExternalID { get; set; }

    public string Code { get; init; }
    
    public string Note { get; init; }

    public string ProductNumber { get; init; }

    public Guid ManufacturerID { get; init; }

    public int? LimitInHours { get; init; }
    
    public bool DowngradeAvailable { get; init; }
    
    public bool IsFull { get; init; }
    
    public bool CanBuy { get; set; }
    
    public bool SoftwareExecutionCountIsDefined { get; init; }
    
    public Guid SoftwareLicenseScheme { get; init; }
    
    public virtual ProductCatalogType ProductCatalogType {get; init; }
    
    public virtual Manufacturer Manufacturer { get; init; }
    
}