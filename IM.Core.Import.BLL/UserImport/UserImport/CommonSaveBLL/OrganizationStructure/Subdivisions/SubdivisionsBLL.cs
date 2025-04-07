using AutoMapper;
using IM.Core.Import.BLL.Interface.OrganizationStructure.Subdivisions;
using InfraManager;
using InfraManager.DAL;
using IM.Core.Import.BLL.Interface.Import;
using InfraManager.DAL.OrganizationStructure;
using Microsoft.Extensions.Logging;
using InfraManager.DAL.Import;
using IM.Core.Import.BLL.Interface;
using IM.Core.Import.BLL.Interface.Import.Log;
using IM.Core.Import.BLL.Interface.Import.Models;

namespace IM.Core.Import.BLL.OrganizationStructure.Subdivisions;

public class SubdivisionsBLL : ISubdivisionsBLL, ISelfRegisteredService<ISubdivisionsBLL>
{
    private readonly IRepository<Subdivision> _repository;
    private readonly IUnitOfWork _saveChanges;
    private readonly ILocalLogger<SubdivisionsBLL> _logger;
    private readonly ISubdivisionsAddRangeQuery _subdivisionsAddRangeQuery;

    private readonly IBaseImportMapper<ISubdivisionDetails, Subdivision> _mapper;
    public SubdivisionsBLL(
        IRepository<Subdivision> repository,
        ISubdivisionsAddRangeQuery subdivisionsAddRangeQuery,
        IUnitOfWork saveChanges,
        ILocalLogger<SubdivisionsBLL> logger,
        IBaseImportMapper<ISubdivisionDetails, Subdivision> mapper)
    {
        _repository = repository;
        _subdivisionsAddRangeQuery = subdivisionsAddRangeQuery;
        _saveChanges = saveChanges;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<int> CreateSubdivisionsAsync(IEnumerable<Subdivision> subdivisions,
        CancellationToken cancellationToken = default)
    {
        var count = 0;
        foreach (var subdivision in subdivisions)
        {
            try
            {
                await _subdivisionsAddRangeQuery.ExecuteAsync(new[] {subdivision}, cancellationToken);

                await _saveChanges.SaveAsync(cancellationToken);
                count++;
            }
            catch (Exception e)
            {
                _logger.Information(
                    $"Ошибка добавления подразделения {subdivision.Name} с ExternalId:{subdivision.ExternalID}");
                _logger.Verbose($"Ошибка:{e}");
            }

        }

        return count;
    }

    public void EnrichSubdivisionForCreate(Subdivision subdivision)
    {
        subdivision.ID = Guid.NewGuid();
    }

    public async Task UpdateAsync(ISubdivisionDetails subdivision, Subdivision entity,
        ImportData<ISubdivisionDetails, Subdivision> importData,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (entity == null) throw new Exception("Subdivision not found");

            _mapper.Map(importData,new[]{(subdivision, entity)});

            await _saveChanges.SaveAsync(cancellationToken);
            _logger.Information($"Обновлено подразделение с именем {subdivision.Name}");

        }
        catch (Exception e)
        {
            _logger.Error("Ошибка обновления подразделения", subdivision.Name, e);
            throw;
        }
    }

    public async Task<int> UpdateSubdivisionsAsync(Dictionary<ISubdivisionDetails, Subdivision> subdivisions,
        ImportData<ISubdivisionDetails, Subdivision> importData,
        CancellationToken cancellationToken = default)
    {
        var maxCount = 10;
        var count = 0;
        foreach (var subdivision in subdivisions)
        {
            _mapper.Map(importData, new[] {(subdivision.Key, subdivision.Value)});
            try
            {
                await _saveChanges.SaveAsync(cancellationToken);
                count++;
            }
            catch (Exception e)
            {
                _logger.Information(
                    $"Ошибка обновления подразделения {subdivision.Value.Name} ExternalID:{subdivision.Value.ExternalID}");
                _logger.Verbose($"Ошибка:{e}");
            }
        }
        
        return count;
    }

    public async Task<Subdivision> GetSubdivisionByIDAsync(Guid id, CancellationToken cancellationToken = default)
        => await _repository.FirstOrDefaultAsync(x => x.ID == id, cancellationToken);

    public async Task<Subdivision> GetSubdivisionByIDOrNameAsync(SubdivisionDetails subdivision, CancellationToken cancellationToken = default)
    {
        var byExternalId = await _repository.With(x=>x.Organization)
            .FirstOrDefaultAsync(
                x => !string.IsNullOrWhiteSpace(x.ExternalID) && x.ExternalID == subdivision.ExternalID,
                cancellationToken);
        if (byExternalId != null)
            return byExternalId;
        Guid? parentId = null;
        bool notFound = false;
        foreach (var subdivisionDetail in subdivision.SubdivisionParent)
        {
            var firstOrDefaultAsync = await _repository
                .FirstOrDefaultAsync(x =>
                                         (!string.IsNullOrWhiteSpace(x.Name) && x.Name == subdivisionDetail) && x.SubdivisionID == parentId, cancellationToken);
            if (firstOrDefaultAsync == null)
            {
                notFound = true;
                break;
            }

            parentId = firstOrDefaultAsync.ID;
        }

        if (notFound)
            return null;
        
        var result = await _repository.With(x=>x.Organization)
            .FirstOrDefaultAsync(x =>(!string.IsNullOrWhiteSpace(x.ExternalID) && x.ExternalID == subdivision.ExternalID) ||
                                     (!string.IsNullOrWhiteSpace(x.Name) && x.Name == subdivision.Name) && x.SubdivisionID == parentId, cancellationToken);

        return result;
    }
}