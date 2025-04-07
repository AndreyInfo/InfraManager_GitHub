using System;

namespace InfraManager.BLL.ProductCatalogue.Models;

public class ProductCatalogModelData
{
    public string Name { get; init; }
    
    public string ExternalID { get; init; }
    
    public string Code { get; init; }

    public string ProductNumber { get; init; }

    public int VendorID { get; init; }

    public string Note { get; init; }
    
    public Guid ProductCatalogTypeID { get; init; }

    public byte[] RowVersion { get; init; }

    public ProductCatalogModelPropertiesData Properties { get; init; }

    //TODO:рассмотреть после реализации форм
    // public string Location { get; set; }
    // public string? Parameters { get; set; }
    //
    // public string SlotTypeName { get; set; }
    //
    // public bool CanBuy { get; set; }
    //
    // public int? PortTypeID { get; set; }
    //
    // public int? PortTechnologyID { get; set; }
    //
    // public int? UnitsID { get; set; }
    //
    // public string? Gost { get; set; }
    //
    // public decimal? Price { get; set; }
    //
    // public decimal? HeightUnit { get; set; }
    //
    // public decimal Width { get; set; }
    //
    // public decimal? Depth { get; set; }
    //
    // public decimal Height { get; set; }
    //
    // public bool? IsMountRack { get; set; }


}