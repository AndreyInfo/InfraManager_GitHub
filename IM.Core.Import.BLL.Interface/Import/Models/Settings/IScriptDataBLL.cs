using InfraManager;

namespace IM.Core.Import.BLL.Interface.Import.Models.Settings;

public interface IScriptDataBLL
{
    Task<ScriptDataDetails<ConcordanceObjectType>[]> GetScriptsAsync(Guid id, CancellationToken token);

    Task<CsvOptions> GetCsvOptions(Guid id, CancellationToken token);
}