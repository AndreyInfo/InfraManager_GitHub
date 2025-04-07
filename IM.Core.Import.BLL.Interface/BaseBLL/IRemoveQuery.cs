namespace IM.Core.Import.BLL.Interface.Import;

public interface IRemoveQuery<TKey, TEntity>
{
    Task RemoveAsync(TKey key, CancellationToken token);
}