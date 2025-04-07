namespace IM.Core.Import.BLL.Interface.Import.Models.Import;

public interface IImporter
{
    Task RunAsync(Guid id, bool withDelete, CancellationToken token);

    Task<IEnumerable<CommonData>> GetNotLoadedDataAsync(Guid id, CancellationToken token);
}