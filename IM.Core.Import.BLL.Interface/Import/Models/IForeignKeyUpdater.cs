namespace IM.Core.Import.BLL.Interface.Import.Models;

public interface IForeignKeyUpdater<TData, in TEntity>
{
    Task<bool> SetFieldsAsync(TData data, TEntity entity, CancellationToken token);
}