using IM.Core.Import.BLL.Interface.Import.Models.FieldUpdater;

namespace IM.Core.Import.BLL.Import.Importer.UploadData;

public class FieldUpdaterChain<TData,TEntity>:IFieldUpdater<TData,TEntity>
{
    private readonly IEnumerable<IFieldUpdater<TData,TEntity>> _updaters;

    public FieldUpdaterChain(params IFieldUpdater<TData,TEntity>[] updaters)
    {
        _updaters = updaters;
    }

    public async Task<bool> SetFieldsAsync(TData data, TEntity entity,
        CancellationToken token)
    {
        foreach (var updater in _updaters)
        {
            if (!await updater.SetFieldsAsync(data, entity, token))
                return false;
        }

        return true;
    }
}