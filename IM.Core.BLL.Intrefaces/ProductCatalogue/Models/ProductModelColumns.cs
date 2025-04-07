using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.ProductCatalogue.Models;

[ListViewItem(ListView.GuidsProductCatalogModelList)]
public class ProductModelColumns
{
    [ColumnSettings(0)]
    [Label(nameof(Resources.ProductCatalogueModel_Class))]
    public string TemplateClassName { get;  }

    [ColumnSettings(1)]
    [Label(nameof(Resources.ProductCatalogueModel_CategoryName))]
    public string CategoryName { get; init; }

    [ColumnSettings(2)]
    [Label(nameof(Resources.ProductCatalogueModel_TypeName))]
    public string ProductCatalogTypeName { get;  }

    [ColumnSettings(3)]
    [Label(nameof(Resources.ProductCatalogueModel_Name))]
    public string Name { get; init; }

    [ColumnSettings(4)]
    [Label(nameof(Resources.ProductCatalogueModel_ProductNumber))]
    public string ProductNumber { get;  }

    [ColumnSettings(5)]
    [Label(nameof(Resources.ProductCatalogueModel_Code))]
    public string Code { get;  }

    [ColumnSettings(6)]
    [Label(nameof(Resources.ProductCatalogueModel_Note))]
    public string Note { get; init; }

    [ColumnSettings(7)]
    [Label(nameof(Resources.ProductCatalogueModel_LifeCycle))]
    public string LifeCycleName { get; init; }

    [ColumnSettings(8)]
    [Label(nameof(Resources.ProductCatalogueModel_ManufacturerName))]
    public string VendorName { get; init; }

    [ColumnSettings(9)]
    [Label(nameof(Resources.ProductCatalogueModel_CanBuy))]
    public bool CanBuy { get; init; }

    [ColumnSettings(10)]
    [Label(nameof(Resources.ProductCatalogueModel_ExternalIdentifier))]
    public string ExternalID { get; init; }

    [ColumnSettings(11)]
    [Label(nameof(Resources.ProductCatalogueModel_IsLogic))]
    public bool IsLogical { get; init; }
}