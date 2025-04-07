using IM.Core.Import.BLL.Import.OSU;
using IM.Core.Import.BLL.Interface;
using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Import.ITAsset;
using IM.Core.Import.BLL.Interface.Import.Models.Settings;
using InfraManager;
using InfraManager.DAL;
using InfraManager.DAL.Import.ITAsset;
using InfraManager.DAL.ITAsset;
using InfraManager.DAL.ProductCatalogue;
using InfraManager.ServiceBase.ImportService.WebAPIModels.ITAsset;
using System.Text;

namespace IM.Core.Import.BLL.Import.Asset;
internal class ITAssetImportModelModificator : IITAssetImportModelModificator, ISelfRegisteredService<IITAssetImportModelModificator>
{
    private const int EncodingWindows1251 = 1251;
    private const string CSVFilesDir = "CSVFILESDIR";
    private const string Num0In = "№";
    private const string Num0Out = "num0";

    private readonly IReadonlyRepository<ITAssetImportCSVConfiguration> _configurationReadonlyRepository;
    private readonly IReadonlyRepository<ITAssetImportCSVConfigurationFieldConcordance> _fieldConcordanceReadonlyRepository;
    private readonly IReadonlyRepository<ITAssetImportCSVConfigurationClassConcordance> _classConcordanceReadonlyRepository;
    private readonly IScriptDataParser<ConcordanceITAssetObjectType> _interpretator;
    private readonly IRepository<ProductCatalogType> _pcTypeRepository;

    public ITAssetImportModelModificator(IReadonlyRepository<ITAssetImportCSVConfiguration> configurationReadonlyRepository,
        IReadonlyRepository<ITAssetImportCSVConfigurationFieldConcordance> fieldConcordanceReadonlyRepository,
        IReadonlyRepository<ITAssetImportCSVConfigurationClassConcordance> classConcordanceReadonlyRepository,
        IScriptDataParser<ConcordanceITAssetObjectType> interpretator,
        IRepository<ProductCatalogType> pcTypeRepository)
    {
        _configurationReadonlyRepository = configurationReadonlyRepository;
        _fieldConcordanceReadonlyRepository = fieldConcordanceReadonlyRepository;
        _classConcordanceReadonlyRepository = classConcordanceReadonlyRepository;
        _interpretator = interpretator;
        _pcTypeRepository = pcTypeRepository;
    }
    public async Task<ITAssetImportDetails[]> GetModelsAsync(IProtocolLogger protocolLogger, ImportTasksDetails importTasksDetails, ITAssetImportSettingData settings, CancellationToken cancellationToken)
    {
        protocolLogger.Information("Подключен источник данных: CSV");
        ITAssetImportCSVConfiguration? configuration = null;
        try
        {
            configuration = await _configurationReadonlyRepository.FirstOrDefaultAsync(x => x.ID == settings.ITAssetImportCSVConfigurationID, cancellationToken);
        }
        catch (Exception e)
        {
            protocolLogger.Error(e, $"Error when getting Configuration for import service it asset with id = {importTasksDetails.ID}");
            throw;
        }
        string[]? csvLines = null;
        try
        {
            csvLines = GetLinesFromCSV(settings.Path).ToArray();
            protocolLogger.AddInputData(InfraManager.ServiceBase.ImportService.Log.ImportInputType.CSV, path: settings.Path);

            protocolLogger.Information($"Прочитано {csvLines.Length - 1} строк(и) данных");
        }
        catch (Exception e)
        {
            protocolLogger.Error(e, $"Error when read file {settings.Path}");
            protocolLogger.Information($"ERR Ошибка чтения файла {settings.Path}");
            throw;
        }

        var fieldEntities = _fieldConcordanceReadonlyRepository.Where(x => x.ITAssetImportCSVConfigurationID == configuration.ID).ToArray();
        var fieldsDetails = GetModelsFromCSV(protocolLogger, csvLines, fieldEntities, configuration.Delimeter);

        var classEntities = _classConcordanceReadonlyRepository.Where(x => x.ITAssetImportCSVConfigurationID == configuration.ID).ToArray();

        foreach (var field in fieldsDetails)
            if (field != null && (field.TypeExternalID != null || field.TypeName != null))
            {
                var classConcordance = classEntities.FirstOrDefault(x => x.Expression == field.TypeExternalID || x.Expression == field.TypeName);
                if (classConcordance == null)
                {
                    protocolLogger.Information($"Не удалось найти сопоставление для типа с идентификатором {field.TypeExternalID} и/или наименованием {field.TypeName}");
                    continue;
                }

                var pcType = await _pcTypeRepository.FirstOrDefaultAsync(x => x.ExternalID == classConcordance.Field || x.Name == classConcordance.Field, cancellationToken);
                if (pcType == null)
                {
                    protocolLogger.Information($"Не удалось найти тип с идентификатором {field.TypeExternalID} и/или наименованием {field.TypeName}");
                    continue;
                }

                field.TypeExternalID = pcType.ExternalID;
                field.TypeName = pcType.Name;
            }

        return fieldsDetails;
    }

    private ITAssetImportDetails[] GetModelsFromCSV(IProtocolLogger protocolLogger, string[] csvLines, ITAssetImportCSVConfigurationConcordance[] fields, string delimeter)
    {
        ScriptDataDetails<ConcordanceITAssetObjectType>[] importDetailsArray = new ScriptDataDetails<ConcordanceITAssetObjectType>[fields.Length];
        for (int i = 0; i < fields.Length; i++)
            importDetailsArray[i] = new ScriptDataDetails<ConcordanceITAssetObjectType>
            {
                FieldEnum = GetEnumByName(fields[i].Field),
                Script = $"record.{ReplaceInvalidSymbols(fields[i].Expression)}",
                ImportTypeEnum = ImportTypeEnum.ITAsset
            };

        var fieldNames = csvLines[0].Split(delimeter).Select(x => ReplaceInvalidSymbols(x)).ToArray();

        ITAssetImportDetails[] importModels = new ITAssetImportDetails[csvLines.Length - 1];
        var scripts = _interpretator.GetParserData(importDetailsArray);
        for (int i = 1; i < csvLines.Length; i++)
        {
            var data = csvLines[i].Split(delimeter);
            var resultInterpretator = _interpretator.ParseToDictionary(scripts, fieldNames, data);
            importModels[i - 1] = ImportHelper.GetModelForITAsset(resultInterpretator);
        }
        protocolLogger.Information($"Загружено {importModels.Length} запись(ей)");
        return importModels;
    }

    private ConcordanceITAssetObjectType GetEnumByName(string field)
        => (ConcordanceITAssetObjectType)System.Enum.Parse(typeof(ConcordanceITAssetObjectType), field);

    private IEnumerable<string> GetLinesFromCSV(string fileName)
    {
        var path = Path.Combine(Environment.GetEnvironmentVariable(CSVFilesDir), fileName);
        using (var reader = File.OpenRead(path))
        {
            using StreamReader streamReader = new StreamReader(reader, CodePagesEncodingProvider.Instance.GetEncoding(EncodingWindows1251));
            while (!streamReader.EndOfStream)
                yield return streamReader.ReadLine();
        }
    }

    private static string ReplaceInvalidSymbols(string str)
        => str.Replace(" ", "_").Replace(Num0In, Num0Out);
}
