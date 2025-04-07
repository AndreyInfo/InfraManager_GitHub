using Inframanager.BLL.EventsOld;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using InfraManager.DAL.ProductCatalogue;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogModels.Models.TerminalModels;

internal sealed class TerminalModelEventBuilder : ProductCatalogModelEventBuilder<TerminalDeviceModel>
    , IConfigureEventBuilder<TerminalDeviceModel>
{
    private const string ModelName = "модель терминального устройства";

    public TerminalModelEventBuilder(
        IFinder<Manufacturer> manufacturers,
        IFinder<ProductCatalogType> productCatalogType) : base(
        ModelName,
        manufacturers,
        productCatalogType)
    {
    }
}