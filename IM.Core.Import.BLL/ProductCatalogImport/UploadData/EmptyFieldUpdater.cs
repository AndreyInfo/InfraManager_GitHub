using IM.Core.Import.BLL.Interface.Import.Models;

namespace IM.Core.Import.BLL.Import.Importer.UploadData;

public class EmptyFieldUpdater<TData,TEntity>:IForeignKeyUpdater<TData,TEntity>
{
    public Task<bool> SetFieldsAsync(TData data, TEntity entity, CancellationToken token)
    {
        return Task.FromResult(true);
    }
}