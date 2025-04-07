using AutoMapper;
using IM.Core.Import.BLL.Interface.Import.Models.Import;
using IM.Core.Import.BLL.Interface.Import.Models.Settings;
using InfraManager;
using InfraManager.DAL;
using Inframanager.DAL.ProductCatalogue.Import;
using InfraManager.DAL.ProductCatalogue.Import;
using Microsoft.EntityFrameworkCore;

namespace IM.Core.Import.BLL;

public class ProductCatalogScriptDataBLL:IScriptDataBLL,ISelfRegisteredService<IScriptDataBLL>
{
    private readonly IReadonlyRepository<ProductCatalogImportCSVConfigurationConcordance> _configurations;
    private readonly IReadonlyRepository<ProductCatalogImportSetting> _importSettings;
    private readonly IMapper _mapper; 
    public ProductCatalogScriptDataBLL(
        IReadonlyRepository<ProductCatalogImportCSVConfigurationConcordance> configurations,
        IMapper mapper, IReadonlyRepository<ProductCatalogImportSetting> importSettings)
    {
        _configurations = configurations;
        _mapper = mapper;
        _importSettings = importSettings;
    }

    public async Task<ScriptDataDetails<ConcordanceObjectType>[]> GetScriptsAsync(Guid id, CancellationToken token)
    { 
        var options =
            await _configurations.Query().Where(x => x.ProductCatalogImportCSVConfiguration.ProductCatalogImportSetting.ID == id)
                .ToArrayAsync(token);

        var result = _mapper.Map<ScriptDataDetails<ConcordanceObjectType>[]>(options);

        return result;
    }

    public async Task<CsvOptions> GetCsvOptions(Guid id, CancellationToken token)
    {
        var optionsSettings = await _importSettings.FirstOrDefaultAsync(x => x.ID == id, token);
        return _mapper.Map<CsvOptions>(optionsSettings);
    }
}