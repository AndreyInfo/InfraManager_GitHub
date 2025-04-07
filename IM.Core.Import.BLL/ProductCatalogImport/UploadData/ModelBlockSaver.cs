using IM.Core.Import.BLL.Interface.Import.Models;
using IM.Core.Import.BLL.Interface.Import.Models.UploadData;
using InfraManager.DAL;

namespace IM.Core.Import.BLL.Import.Importer.UploadData;

public class ModelBlockSaver<TData, TEntity>:IBlockSaver<TData,TEntity>
{
    private readonly IForeignKeyUpdater<TData,TEntity> _fieldUpdater;
    private readonly IBulkInsertOrUpdate<TEntity> _insertOrUpdate;
    
    public ModelBlockSaver(IForeignKeyUpdater<TData, TEntity> fieldUpdater, IBulkInsertOrUpdate<TEntity> insertOrUpdate)
    {
        _fieldUpdater = fieldUpdater;
        _insertOrUpdate = insertOrUpdate;
    }

    public async Task SaveAsync(List<(TEntity entity, TData row)> buffer, CancellationToken token = default)
    {
        foreach (var entity in buffer)
        {
            await _fieldUpdater.SetFieldsAsync(entity.Item2, entity.Item1, token);
        }
        await _insertOrUpdate.ExecuteAsync(buffer.Select(x=>x.Item1), token);
    }
}