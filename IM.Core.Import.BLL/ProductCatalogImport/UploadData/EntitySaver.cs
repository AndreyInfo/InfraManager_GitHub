using IM.Core.Import.BLL.Interface.Import.Models.FieldUpdater;
using IM.Core.Import.BLL.Interface.Import.Models.Import;
using IM.Core.Import.BLL.Interface.Import.Models.UploadData;
using InfraManager.DAL;
using Microsoft.EntityFrameworkCore;
using IImportEntity = InfraManager.DAL.IImportEntity;

namespace IM.Core.Import.BLL.Import.Importer.UploadData;

public class EntitySaver<TData,TEntity>:IEntitySaver<TData,TEntity,CommonData>
where TEntity:class,IImportEntity, new()
{
    private readonly IRepository<TEntity> _entities;
    private readonly IBulkDelete<TEntity> _bulkDelete;
    private readonly IBlockSaver<TData,TEntity> _blockSaver;
    private readonly IFieldUpdater<TData, TEntity> _fieldUpdater;
    
    private readonly List<(TEntity,TData)> _entityBuffer = new();
    private readonly HashSet<string> _externalIds = new();
    

    public EntitySaver(
         IRepository<TEntity> entities,
         IBulkDelete<TEntity> bulkDelete, 
        IBlockSaver<TData, TEntity> blockSaver, 
        IFieldUpdater<TData, TEntity> fieldUpdater
        )
    {
         _entities = entities;
         _bulkDelete = bulkDelete;
        _blockSaver = blockSaver;
        _fieldUpdater = fieldUpdater;
    }

    //todo: проверить дублирующиеся записи в таблицах
    public async Task<bool> AddToBatchDataAsync(TData data, CancellationToken token)
    {
        var entityID = await GetCurrentIdsAsync(token);
        var nameHash = entityID.ToLookup(x => x.Name);

        var entity = new TEntity();
        
        if (!await _fieldUpdater.SetFieldsAsync(data, entity, token)) return false;
        if (entity.ExternalID == null)
            entity.ExternalID = entity.Name;
        {
            var currentNameData = nameHash[entity.Name];
            if (currentNameData.Select(x=>x.ExternalID).Contains( entity.ExternalID))
            {
                var i = 0;
                var name = entity.Name;
                var currentName = name + i;
                var nm = nameHash.ToDictionary(x => x.Key, x => x);
                while (nm.ContainsKey(currentName))
                {
                    i++;
                    currentName = name + i;
                }

                entity.Name = currentName;
            }
        }

        _externalIds.Add(entity.ExternalID);
        _entityBuffer.Add((entity, data));
        return true;
    }

    

    private async Task<(string ExternalID, string Name)[]> GetCurrentIdsAsync(CancellationToken token)
    {
        //TODO:закэшировать
        var entityID = await _entities.Query().Select(x => new {x.ExternalID, x.Name})
            .ToArrayAsync(token);
        return entityID.Select(x=>(x.ExternalID,x.Name)).ToArray();
    }

    public async Task SaveBatchAsync(CancellationToken token)
    {
        await _blockSaver.SaveAsync(_entityBuffer, token);
        _entityBuffer.Clear();
    }

    public async Task DeleteAsync(CancellationToken token)
    {
        var entities = await GetCurrentIdsAsync(token);
        var baseExternalIDHash = entities.Select(x => x.ExternalID)
            .ToHashSet();
        var toRemove = baseExternalIDHash.Except(_externalIds).ToList();
        await _bulkDelete.DeleteExternalIDAsync(toRemove, token);
    }
}