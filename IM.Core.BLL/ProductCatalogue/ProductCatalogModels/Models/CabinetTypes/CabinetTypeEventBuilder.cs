using Inframanager.BLL.EventsOld;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using InfraManager.DAL.ProductCatalogue;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogModels.Models.CabinetTypes;

internal sealed class CabinetTypeEventBuilder : ProductCatalogModelEventBuilder<CabinetType>
    , IConfigureEventBuilder<CabinetType>
{
    private const string ModelName = "модель шкафа";

    public CabinetTypeEventBuilder(
        IFinder<Manufacturer> manufacturers,
        IFinder<ProductCatalogType> productCatalogType) : base(ModelName,
        manufacturers,
        productCatalogType)
    {
    }
}