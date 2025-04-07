using IM.Core.Import.BLL.Interface.Import.Models;
using IM.Core.Import.BLL.Interface.Import.Models.Settings;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using Microsoft.Extensions.Caching.Memory;
using IImportEntity = InfraManager.DAL.IImportEntity;

namespace IM.Core.Import.BLL.Import.Importer.UploadData;

public class ModelForeignKeyUpdater<TData,TEntity> : IForeignKeyUpdater<TData, TEntity>, IDisposable
    where TEntity : IImportEntity,IImportPostLinkParameters
    where TData:IModelDataTryGet
{
    private const string CacheKeyBase = "ManufacturersIdCache";
    private readonly IReadonlyRepository<Manufacturer> _repository;
    private readonly IMemoryCache _cache;

    public ModelForeignKeyUpdater(IReadonlyRepository<Manufacturer> repository, IMemoryCache cache)
    {
        _repository = repository;
        _cache = cache;
    }

    public async Task<bool> SetFieldsAsync(TData data, TEntity entity, CancellationToken token)
    {
        
        data.TryGetValue(CommonFieldNames.ManufacturerExternalID, out var externalID);
        data.TryGetValue(CommonFieldNames.ManufacturerName, out var externalName);
        if (!_cache.TryGetValue(CacheKeyBase, out Dictionary<(string?, string?), int> manufacturerKeys))
            manufacturerKeys = new();
        if (!manufacturerKeys.TryGetValue((externalID, externalName), out var manufacturerId))
        {
            Manufacturer? manufacturer = null;
            var notEmptyExternalId = !string.IsNullOrWhiteSpace(externalID);
            if (notEmptyExternalId)
                manufacturer = await _repository.FirstOrDefaultAsync(x => x.ExternalID == externalID, token);
            if (manufacturer == null && notEmptyExternalId)
                manufacturer = await _repository.FirstOrDefaultAsync(x => x.Name == externalName, token);
            else
                return false;
            if (manufacturer == null)
                return false;
            manufacturerKeys[(externalID, externalName)] = manufacturer.ID;
        }

        _cache.Set(CacheKeyBase, manufacturerKeys);

        entity.ManufacturerID = manufacturerId;

        return true;
    }

    public void Dispose()
    {
        _cache.Remove(CacheKeyBase);
    }
}