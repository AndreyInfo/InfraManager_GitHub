using System.Text;
using IM.Core.Import.BLL.Interface.Import.CSV;
using IM.Core.Import.BLL.Interface.Import.Models.Import;
using IM.Core.Import.BLL.Interface.Import.Models.Settings;
using InfraManager;

namespace IM.Core.Import.BLL.Import.Csv;

public class CsvRepository:ICsvRepository,ISelfRegisteredService<ICsvRepository>
{
    private const string EncodingName = "windows-1251";
    private readonly IScriptDataBLL _scriptDataBLL;

    public CsvRepository(IScriptDataBLL scriptDataBLL)
    {
        _scriptDataBLL = scriptDataBLL;
    }

    public async Task<StreamReader> GetCsvStreamReader(Guid id, CancellationToken token = default)
    {
        var optionsSettings = await _scriptDataBLL.GetCsvOptions(id, token);
        
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        
        var encoding = Encoding.GetEncoding(EncodingName);

        var stream = File.Open(optionsSettings.Path, FileMode.Open, FileAccess.Read, FileShare.Read);

        var fileReader = new StreamReader(
            stream,
            encoding,
            true,
            2000000,
            false);
        return fileReader;
    }
}