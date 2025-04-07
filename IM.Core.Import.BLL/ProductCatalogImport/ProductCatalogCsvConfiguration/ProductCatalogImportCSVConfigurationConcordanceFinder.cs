using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Import.Models.ModelSettings;
using InfraManager;
using InfraManager.DAL;
using Inframanager.DAL.ProductCatalogue.Import;

namespace IM.Core.Import.BLL.Import.SettingsBLL.ImportSettings.ProductCatalogImportSettings;

public class ProductCatalogImportCSVConfigurationConcordanceFinder:
    IFinderQuery<ProductCatalogImportCSVConcordanceKey ,ProductCatalogImportCSVConfigurationConcordance>,
    ISelfRegisteredService<IFinderQuery<ProductCatalogImportCSVConcordanceKey, ProductCatalogImportCSVConfigurationConcordance>>
{
    private readonly IReadonlyRepository<ProductCatalogImportCSVConfigurationConcordance> _repository;

    public ProductCatalogImportCSVConfigurationConcordanceFinder(IReadonlyRepository<ProductCatalogImportCSVConfigurationConcordance> repository)
    {
        _repository = repository;
    }

    public Task<ProductCatalogImportCSVConfigurationConcordance> GetFindQueryAsync(ProductCatalogImportCSVConcordanceKey key, CancellationToken token)
    {
        return _repository.FirstOrDefaultAsync(x => x.ID == key.ID && x.Field == key.Field, token);
    }
}