namespace IM.Core.Import.BLL.Interface.Import.Models.UploadData;

public interface ISaver<TData>
{
    Task<bool> AddToBatchDataAsync(TData data, CancellationToken token);
    Task SaveBatchAsync(CancellationToken token);

    Task DeleteAsync(CancellationToken token);
}

public interface IModelSaver<TData> : ISaver<TData>
{
    
}

public interface IEntitySaver<TData,TEntity> : ISaver<TData>
{
}

public interface IEntitySaver<TData, TEntity, TCommonData> : IEntitySaver<TData, TEntity>
{
}