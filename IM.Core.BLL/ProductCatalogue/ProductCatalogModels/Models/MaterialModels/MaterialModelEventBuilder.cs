using Inframanager.BLL.EventsOld;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using InfraManager.DAL.ProductCatalogue;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogModels.Models.MaterialModels;

internal sealed class MaterialModelEventBuilder : ProductCatalogModelEventBuilder<MaterialModel>
    , IConfigureEventBuilder<MaterialModel>
{
    private const string ModelName = "расходный матреиал";

    public MaterialModelEventBuilder(IFinder<Manufacturer> manufacturers,
        IFinder<ProductCatalogType> productCatalogType) :
        base(ModelName,
        manufacturers,
        productCatalogType)
    {
    }
}