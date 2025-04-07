using Inframanager.BLL;
using InfraManager.DAL;
using Inframanager.DAL.ProductCatalogue.Import;

namespace InfraManager.BLL.ProductCatalogue.Import;

public class ProductCatalogImportSettingTypesQuery : IBuildEntityQuery<ProductCatalogImportSettingTypes,
        ProductCatalogImportSettingTypesOutputDetails, ProductCatalogImportSettingTypesFilter>,
    ISelfRegisteredService<IBuildEntityQuery<ProductCatalogImportSettingTypes,
        ProductCatalogImportSettingTypesOutputDetails, ProductCatalogImportSettingTypesFilter>>
{
    private readonly IReadonlyRepository<ProductCatalogImportSettingTypes> _repository;

    public ProductCatalogImportSettingTypesQuery(IReadonlyRepository<ProductCatalogImportSettingTypes> repository)
    {
        _repository = repository;
    }

    public IExecutableQuery<ProductCatalogImportSettingTypes> Query(ProductCatalogImportSettingTypesFilter filter)
    {
        var query = _repository
            .Query();


        return query;
    }
}