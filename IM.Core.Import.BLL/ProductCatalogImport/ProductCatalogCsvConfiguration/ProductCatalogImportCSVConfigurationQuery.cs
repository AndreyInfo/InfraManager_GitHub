using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Import.Models.ProductCatalogCsvConfiguration;
using InfraManager;
using InfraManager.DAL;
using InfraManager.DAL.ProductCatalogue.Import;

namespace IM.Core.Import.BLL.Import.SettingsBLL.ImportSettings.ProductCatalogCsvConfiguration;

public class ProductCatalogImportCSVConfigurationQuery : IFilterEntity<ProductCatalogImportCSVConfiguration,
        ProductCatalogImportCSVConfigurationFilter>,
    ISelfRegisteredService<IFilterEntity<ProductCatalogImportCSVConfiguration,
        ProductCatalogImportCSVConfigurationFilter>>
{
    private readonly IReadonlyRepository<ProductCatalogImportCSVConfiguration> _repository;

    public ProductCatalogImportCSVConfigurationQuery(
        IReadonlyRepository<ProductCatalogImportCSVConfiguration> repository)
    {
        _repository = repository;
    }

    public IQueryable<ProductCatalogImportCSVConfiguration> Query(
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
        

        return query;
    }
}