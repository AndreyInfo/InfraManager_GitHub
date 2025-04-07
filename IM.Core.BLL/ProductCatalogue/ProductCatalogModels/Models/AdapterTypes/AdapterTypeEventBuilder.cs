using Inframanager.BLL.EventsOld;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using InfraManager.DAL.ProductCatalogue;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogModels.Models.AdapterTypes;

public class AdapterTypeEventBuilder : ProductCatalogModelEventBuilder<AdapterType>
    , IConfigureEventBuilder<AdapterType>
{
    private const string ModelName = "модель адаптера";

    public AdapterTypeEventBuilder(
        IFinder<Manufacturer> manufacturers,
        IFinder<ProductCatalogType> productCatalogType) : base(ModelName, manufacturers, productCatalogType)
    {
    }
}