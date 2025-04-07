using IM.Core.Import.BLL.Interface.Import.CSV;
using IM.Core.Import.BLL.Interface.Import.Models;
using IM.Core.Import.BLL.Interface.Import.Models.Import;
using IM.Core.Import.BLL.Interface.Import.Models.Settings;
using InfraManager.DAL;
using Inframanager.DAL.ProductCatalogue.Units;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using InfraManager;

namespace IM.Core.Import.BLL.Import.Importer.DownloadData;

public class UnitsValidator<TData>:IValidator<TData,Unit>,IDisposable
where TData:IModelDataTryGet
{
    private const string CacheKey = "UnitValidatorExceptions";
    private const string PropertyNameCacheKey = "UnitValidatorExceptions_propertyName";
    private readonly IReadonlyRepository<Unit> _unitsRepository;
    private readonly ICommonPropertyData<Unit, CommonData> _typeCommonPropertyData;
    private readonly IMemoryCache _cache;
    private readonly ICsvRepository _repository;
    private readonly ICsvStringReader _reader;
    private readonly IScriptDataParser<ConcordanceObjectType> _parser;
    private readonly IScriptDataBLL _scriptDataBLL;

    public UnitsValidator(IReadonlyRepository<Unit> unitsRepository,
        ICommonPropertyData<Unit, CommonData> typeCommonPropertyData,
        IMemoryCache cache,
        ICsvRepository repository,
        ICsvStringReader reader, 
        IScriptDataParser<ConcordanceObjectType> parser, 
        IScriptDataBLL scriptDataBLL)
    {
        _unitsRepository = unitsRepository;
        _typeCommonPropertyData = typeCommonPropertyData;
        _cache = cache;
        _repository = repository;
        _reader = reader;
        _parser = parser;
        _scriptDataBLL = scriptDataBLL;
    }

    public async Task<bool> ValidateAsync(Guid id, TData model, CancellationToken token)
    {
        //todo: сделать конфигурацию для кэша
        if (!_cache.TryGetValue(PropertyNameCacheKey, out string? dictionaryKey))
        {
            dictionaryKey = _typeCommonPropertyData.GetKeyName(nameof(Unit), nameof(Unit.Name));
            _cache.Set(PropertyNameCacheKey, dictionaryKey, TimeSpan.FromMinutes(10));
        }

        if (string.IsNullOrWhiteSpace(dictionaryKey))
            return true;

        if (!model.TryGetValue(dictionaryKey, out var externalId))
            return true;
        
        if (!_cache.TryGetValue(CacheKey, out HashSet<string> exceptionIds))
        {
            exceptionIds = await LoadExceptionsHashAsync(id, token);
            _cache.Set(CacheKey, exceptionIds, TimeSpan.FromMinutes(10));
        }

        return exceptionIds.Contains(externalId);
    }

    private async Task<HashSet<string>> LoadExceptionsHashAsync(Guid id, CancellationToken token)
    {
        var externalIdData = _typeCommonPropertyData.GetKeyName(nameof(Unit), nameof(Unit.ExternalID));
        if (string.IsNullOrWhiteSpace(externalIdData))
            return new HashSet<string>();

        var externalName = _typeCommonPropertyData.GetKeyName(nameof(Unit), nameof(Unit.Name));
        if (string.IsNullOrWhiteSpace(externalName))
            return new HashSet<string>();
        
        var units = await _unitsRepository.Query()
            .Select(x => new {x.ExternalID, x.Name})
            .ToListAsync(token);

        var options = await _scriptDataBLL.GetCsvOptions(id, token);
        var delimiter = options.Delimeter;
        
        var csvStream = (await _repository.GetCsvStreamReader(id, token)) ??
                        throw new IOException("Не могу открыть csv файл для чтения");
        string[] header = { };
        
        if (!csvStream.EndOfStream)
            header = _reader.GetData(csvStream, delimiter).ToArray();
        var scripts = await _scriptDataBLL.GetScriptsAsync(id, token);
        var scriptsInfo = _parser.GetParserData(scripts);
        while (!csvStream.EndOfStream)
        {
            var csvData = _reader.GetData(csvStream, delimiter).ToArray();
            var data =_parser.ParseToDictionary( scriptsInfo,header,csvData);
            
            if (!data.TryGetValue(externalIdData, out var externalId) || string.IsNullOrWhiteSpace(externalId))
                continue;

            if (!data.TryGetValue(externalName, out var name) || string.IsNullOrWhiteSpace(name))
                continue;

            units.Add(new {ExternalID = externalId, Name = name});
        }

        var exceptIds = units.GroupBy(x => x.ExternalID)
            .Where(x => x.Select(unitData => unitData.Name).Distinct().Count() > 1)
            .Select(x => x.Key).ToHashSet();
        return exceptIds;
    }

    public void Dispose()
    {
        _cache.Remove(CacheKey);
        _cache.Remove(PropertyNameCacheKey);
    }
}