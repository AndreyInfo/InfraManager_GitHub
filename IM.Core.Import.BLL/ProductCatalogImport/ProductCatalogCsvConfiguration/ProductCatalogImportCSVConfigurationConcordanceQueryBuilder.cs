using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Import.Models.ModelSettings;
using InfraManager;
using InfraManager.DAL;
using Inframanager.DAL.ProductCatalogue.Import;

namespace IM.Core.Import.BLL.Import.SettingsBLL.ImportSettings;

public class ProductCatalogImportCSVConfigurationConcordanceQueryBuilder : IFilterEntity<
        ProductCatalogImportCSVConfigurationConcordance, 
        ProductCatalogImportCSVConfigurationConcordanceFilter>,
    ISelfRegisteredService<IFilterEntity<ProductCatalogImportCSVConfigurationConcordance,
        ProductCatalogImportCSVConfigurationConcordanceFilter>>
{
    private readonly IReadonlyRepository<ProductCatalogImportCSVConfigurationConcordance> _repository;

    public ProductCatalogImportCSVConfigurationConcordanceQueryBuilder(
        IReadonlyRepository<ProductCatalogImportCSVConfigurationConcordance> repository)
    {
        _repository = repository;
    }

    public IQueryable<ProductCatalogImportCSVConfigurationConcordance> Query(
        ProductCatalogImportCSVConfigurationConcordanceFilter filter)
    {
        var query = _repository
            .Query();

        if (!string.IsNullOrEmpty(filter.Expression))
            query = query.Where(x => x.Expression.Contains(filter.Expression));


        return query;
    }
}