namespace IM.Core.Import.BLL.Interface.Import.Models.FieldUpdater;

public interface IFieldUpdater<TData,in TEntity>
{
    Task<bool> SetFieldsAsync(TData data, TEntity entity, CancellationToken token);
}