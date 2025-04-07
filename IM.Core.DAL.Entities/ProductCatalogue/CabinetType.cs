using System;
using Inframanager;
using InfraManager.DAL.Asset;

namespace InfraManager.DAL.ProductCatalogue;

[ObjectClassMapping(ObjectClass.CabinetType)]
[OperationIdMapping(ObjectAction.Insert, OperationID.CabinetType_Insert)]
[OperationIdMapping(ObjectAction.Update, OperationID.CabinetType_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.CabinetType_Delete)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.CabinetType_Details)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.None)]
public class CabinetType : IProductModel<int>
{
    public int ID { get; init; }

    public string Name { get; init; }

    public string ExternalID { get; init; }

    public int ManufacturerID { get; init; }

    public int VerticalSize { get; init; }

    public decimal DepthSize { get; init; }

    public byte[] Image { get; init; }

    public string ProductNumberCyrillic { get; init; }

    public string Category { get; init; }

    public string Code { get; init; }

    public string Note { get; init; }

    public string ProductNumber { get; init; }

    public Guid IMObjID { get; init; }

    public int? ComplementaryID { get; init; }

    public decimal WidthI { get; init; }

    public decimal? Height { get; init; }

    public decimal? Width { get; init; }

    public short NumberingScheme { get; init; }

    public byte[] RowVersion { get; init; }

    public Guid ProductCatalogTypeID { get; init; }

    public virtual ProductCatalogType ProductCatalogType { get; set; }

    public virtual Manufacturer Manufacturer { get; init; }
}
