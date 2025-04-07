using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Import.Models.ModelSettings;
using InfraManager;
using InfraManager.DAL;
using Inframanager.DAL.ProductCatalogue.Import;

namespace IM.Core.Import.BLL.Import.SettingsBLL.ImportSettings;

public class ProductCatalogImportSettingTypesQuery : IFilterEntity<ProductCatalogImportSettingTypes,
        ProductCatalogImportSettingTypesFilter>,
    ISelfRegisteredService<IFilterEntity<ProductCatalogImportSettingTypes,
        ProductCatalogImportSettingTypesFilter>>
{
    private readonly IReadonlyRepository<ProductCatalogImportSettingTypes> _repository;

    public ProductCatalogImportSettingTypesQuery(IReadonlyRepository<ProductCatalogImportSettingTypes> repository)
    {
        _repository = repository;
    }

    public IQueryable<ProductCatalogImportSettingTypes> Query(ProductCatalogImportSettingTypesFilter filter)
    {
        var query = _repository
            .Query();


        return query;
    }
}