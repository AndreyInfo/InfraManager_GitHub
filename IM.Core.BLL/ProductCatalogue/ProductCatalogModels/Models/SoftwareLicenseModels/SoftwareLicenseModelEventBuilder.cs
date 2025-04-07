using Inframanager.BLL.EventsOld;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using InfraManager.DAL.ProductCatalogue;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogModels.Models.SoftwareLicenseModels;

internal sealed class SoftwareLicenseModelEventBuilder : ProductCatalogModelEventBuilder<SoftwareLicenseModel>
    , IConfigureEventBuilder<SoftwareLicenseModel>
{
    private const string ModelName = "модель лицензии программного обеспечения";

    public SoftwareLicenseModelEventBuilder(
        IFinder<Manufacturer> manufacturers,
        IFinder<ProductCatalogType> productCatalogType)
        : base(ModelName,
        manufacturers,
        productCatalogType)
    {
    }
}