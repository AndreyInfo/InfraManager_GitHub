using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Import.Models.ProductCatalogImportSettings;
using InfraManager;
using InfraManager.DAL;
using InfraManager.DAL.ProductCatalogue.Import;

namespace IM.Core.Import.BLL.Import.SettingsBLL.ImportSettings.ProductCatalogImportSettings;

public class ProductCatalogImportSettingQuery : IFilterEntity<ProductCatalogImportSetting,
        ProductCatalogImportSettingFilter>,
    ISelfRegisteredService<IFilterEntity<ProductCatalogImportSetting, 
        ProductCatalogImportSettingFilter>>
{
    private readonly IReadonlyRepository<ProductCatalogImportSetting> _repository;

    public ProductCatalogImportSettingQuery(IReadonlyRepository<ProductCatalogImportSetting> repository)
    {
        _repository = repository;
    }

    public IQueryable<ProductCatalogImportSetting> Query(ProductCatalogImportSettingFilter filter)
    {
        var query = _repository
            .Query();

        if (!string.IsNullOrEmpty(filter.Name))
            query = query.Where(x => x.Name.Contains(filter.Name));
        
        if (!string.IsNullOrEmpty(filter.Note))
            query = query.Where(x => x.Note.Contains(filter.Note));
        
        if (filter.RestoreRemovedModels != null)
            query = query.Where(x => x.RestoreRemovedModels == filter.RestoreRemovedModels);
        
        if (filter.TechnologyTypeID.HasValue)
            query = query.Where(x => x.TechnologyTypeID == filter.TechnologyTypeID);
        
        if (filter.JackTypeID.HasValue)
            query = query.Where(x => x.JackTypeID == filter.JackTypeID);
        
        if (filter.ProductCatalogImportCSVConfigurationID.HasValue)
            query = query.Where(x =>
                x.ProductCatalogImportCSVConfigurationID == filter.ProductCatalogImportCSVConfigurationID);
        
        if (!string.IsNullOrEmpty(filter.Path))
            query = query.Where(x => x.Path.Contains(filter.Path));
        
    
        return query;
    }
}