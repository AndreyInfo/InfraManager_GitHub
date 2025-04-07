using Inframanager.BLL.EventsOld;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using InfraManager.DAL.ProductCatalogue;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogModels.Models.NetworkDeviceModels;

internal sealed class NetworkDeviceModelEventBuilder : ProductCatalogModelEventBuilder<NetworkDeviceModel>
    , IConfigureEventBuilder<NetworkDeviceModel>
{
    private const string ModelName = "модель сетевого оборудования";

    public NetworkDeviceModelEventBuilder(
        IFinder<Manufacturer> manufacturers,
        IFinder<ProductCatalogType> productCatalogType) : base(ModelName,
        manufacturers,
        productCatalogType)
    {
    }
}