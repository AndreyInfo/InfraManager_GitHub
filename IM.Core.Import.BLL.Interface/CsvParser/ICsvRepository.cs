namespace IM.Core.Import.BLL.Interface.Import.CSV;

public interface ICsvRepository
{
    Task<StreamReader> GetCsvStreamReader(Guid id, CancellationToken token = default);
}