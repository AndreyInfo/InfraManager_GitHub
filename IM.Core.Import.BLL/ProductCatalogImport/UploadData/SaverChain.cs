using IM.Core.Import.BLL.Interface.Import.Models.UploadData;

namespace IM.Core.Import.BLL.Import.Importer.UploadData;

//TODO: реализовать слабое связывание
public class SaverChain<TData>:ISaver<TData>
{
    private readonly ISaver<TData>[] _savers;

    public SaverChain(params ISaver<TData>[] savers)
    {
        _savers = savers;
    }

    public async Task<bool> AddToBatchDataAsync(TData data, CancellationToken token)
    {
        foreach (var saver in _savers)
        {
           await saver.AddToBatchDataAsync(data, token); 
        }

        return true;
    }

    public async Task SaveBatchAsync(CancellationToken token)
    {
        foreach (var saver in _savers)
        {
            await saver.SaveBatchAsync(token);
        }
    }

    public async Task DeleteAsync(CancellationToken token)
    {
        foreach (var saver in _savers.Reverse())
        {
            await saver.DeleteAsync(token);
        }
    }
}