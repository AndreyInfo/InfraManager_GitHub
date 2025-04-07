using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inframanager.DAL.ProductCatalogue.Import;
using Microsoft.EntityFrameworkCore;
using InfraManager.DAL.ProductCatalogue.Import;

namespace InfraManager.DAL.Import;

public class GetValidTypesQuery:IGetValidTypesQuery,ISelfRegisteredService<IGetValidTypesQuery>
{
    private readonly IRepository<ProductCatalogImportSettingTypes> _importSettingTypes;

    public GetValidTypesQuery(IRepository<ProductCatalogImportSettingTypes> importSettingTypes)
    {
        _importSettingTypes = importSettingTypes;
    }

    public async Task<Guid[]> ExecuteAsync(Guid importSettingsID, CancellationToken token)
    {
        var data = await _importSettingTypes.ToArrayAsync(x=>x.ProductCatalogImportSettingID==importSettingsID, token);
        return data.Select(x => x.ProductCatalogTypeID).ToArray();
    }
}