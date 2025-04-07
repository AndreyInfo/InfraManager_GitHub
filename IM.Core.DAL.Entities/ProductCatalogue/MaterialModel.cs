using System;
using Inframanager;
using Inframanager.DAL.ProductCatalogue.Units;
using InfraManager.DAL.Asset;

namespace InfraManager.DAL.ProductCatalogue;

[ObjectClassMapping(ObjectClass.MaterialModel)]
[OperationIdMapping(ObjectAction.Insert, OperationID.MaterialModel_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.MaterialModel_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.MaterialModel_Delete)]
[OperationIdMapping(ObjectAction.ViewDetails,OperationID.MaterialModel_Properties)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.None)]
public class MaterialModel : IProductModel<int>, IImportModel
{
    public Guid IMObjID { get; init; }

    public int ManufacturerID { get; set; }

    public Guid? UnitID { get; init; }
    
    public string Name { get; set; }
    
    public string Note { get; set; }

    public string ExternalID { get; set; }
    
    public string Code { get; init; }

    public string Gost { get; init; }
    
    public decimal? CartridgeResource { get; init; }
    
    public Guid? CartrigeTypeID { get; init; }
    
    public decimal? Cost { get; init; }
    
    public Guid? ComplementaryID { get; init; }
    
    public Guid ProductCatalogTypeID { get; set; }
    
    public byte[] RowVersion { get; init; }

    public string ProductNumber { get; set; }

    public bool CanBuy { get; set; }

    public bool Removed { get; init; }

    public virtual ProductCatalogType ProductCatalogType { get; init; }
    public virtual Manufacturer Manufacturer { get; init; }
    public virtual CartridgeType CartridgeType { get; init; }
    public virtual Unit Unit { get; init; }
}