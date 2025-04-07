using IM.Core.Import.BLL.Interface.Import.Models.UploadData;
using InfraManager.DAL;

namespace IM.Core.Import.BLL.Import.Importer.UploadData;

public class DefaultBlockSaver<TData,TEntity>:IBlockSaver<TData,TEntity>
{
    private readonly IBulkInsertOrUpdate<TEntity> _bulkInsertOrUpdate;

    public DefaultBlockSaver(IBulkInsertOrUpdate<TEntity> bulkInsertOrUpdate)
    {
        _bulkInsertOrUpdate = bulkInsertOrUpdate;
    }

    public Task SaveAsync(List<(TEntity entity, TData row)> buffer, CancellationToken token = default)
    {
        return _bulkInsertOrUpdate.ExecuteAsync(buffer.Select(x => x.entity), token);
    }
}