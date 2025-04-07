using Inframanager.BLL.EventsOld;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using InfraManager.DAL.ProductCatalogue;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogModels.Models.PeripherialTypes;

internal sealed class PeripherialTypeEventBuilder : ProductCatalogModelEventBuilder<PeripheralType>
    , IConfigureEventBuilder<PeripheralType>
{
    private const string ModelName = "модель периферийного оборудования";

    public PeripherialTypeEventBuilder(
        IFinder<Manufacturer> manufacturers,
        IFinder<ProductCatalogType> productCatalogType) : base(ModelName,
        manufacturers,
        productCatalogType)
    {
    }
}