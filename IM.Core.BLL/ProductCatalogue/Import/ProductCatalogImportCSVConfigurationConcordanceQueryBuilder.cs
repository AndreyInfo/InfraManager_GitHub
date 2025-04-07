using Inframanager.BLL;
using InfraManager.DAL;
using Inframanager.DAL.ProductCatalogue.Import;

namespace InfraManager.BLL.ProductCatalogue.Import;

public class ProductCatalogImportCSVConfigurationConcordanceQueryBuilder : IBuildEntityQuery<
        ProductCatalogImportCSVConfigurationConcordance, ProductCatalogImportCSVConfigurationConcordanceOutputDetails,
        ProductCatalogImportCSVConfigurationConcordanceFilter>,
    ISelfRegisteredService<IBuildEntityQuery<ProductCatalogImportCSVConfigurationConcordance,
        ProductCatalogImportCSVConfigurationConcordanceOutputDetails,
        ProductCatalogImportCSVConfigurationConcordanceFilter>>
{
    private readonly IReadonlyRepository<ProductCatalogImportCSVConfigurationConcordance> _repository;

    public ProductCatalogImportCSVConfigurationConcordanceQueryBuilder(
        IReadonlyRepository<ProductCatalogImportCSVConfigurationConcordance> repository)
    {
        _repository = repository;
    }

    public IExecutableQuery<ProductCatalogImportCSVConfigurationConcordance> Query(
        ProductCatalogImportCSVConfigurationConcordanceFilter filter)
    {
        var query = _repository
            .Query();

        if (!string.IsNullOrEmpty(filter.Expression))
            query = query.Where(x => x.Expression.Contains(filter.Expression));


        return query;
    }
}