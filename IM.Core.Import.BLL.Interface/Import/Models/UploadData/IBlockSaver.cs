namespace IM.Core.Import.BLL.Interface.Import.Models.UploadData;

public interface IBlockSaver<TData, TEntity>
{
    Task SaveAsync(List<(TEntity entity, TData row)> buffer, CancellationToken token = default);
}