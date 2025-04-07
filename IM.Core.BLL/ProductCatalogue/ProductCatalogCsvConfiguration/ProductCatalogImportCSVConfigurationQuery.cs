using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.ProductCatalogue.Import;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogCsvConfiguration;

public class ProductCatalogImportCSVConfigurationQuery : IBuildEntityQuery<ProductCatalogImportCSVConfiguration,
        ProductCatalogImportCSVConfigurationOutputDetails, ProductCatalogImportCSVConfigurationFilter>,
    ISelfRegisteredService<IBuildEntityQuery<ProductCatalogImportCSVConfiguration,
        ProductCatalogImportCSVConfigurationOutputDetails, ProductCatalogImportCSVConfigurationFilter>>
{
    private readonly IReadonlyRepository<ProductCatalogImportCSVConfiguration> _repository;

    public ProductCatalogImportCSVConfigurationQuery(
        IReadonlyRepository<ProductCatalogImportCSVConfiguration> repository)
    {
        _repository = repository;
    }

    public IExecutableQuery<ProductCatalogImportCSVConfiguration> Query(
        ProductCatalogImportCSVConfigurationFilter filter)
    {
        var query = _repository
            .Query();

        if (!string.IsNullOrEmpty(filter.Name))
            query = query.Where(x => x.Name.Contains(filter.Name));

        if (!string.IsNullOrEmpty(filter.Note))
            query = query.Where(x => x.Note.Contains(filter.Note));

        if (filter.Delimeter != null)
            query = query.Where(x => x.Delimeter == filter.Delimeter);

        if (filter.RowVersion != null)
            query = query.Where(x => x.RowVersion == filter.RowVersion);


        return query;
    }
}