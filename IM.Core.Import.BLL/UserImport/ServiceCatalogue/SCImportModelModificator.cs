using IM.Core.Import.BLL.Import.OSU;
using IM.Core.Import.BLL.Interface;
using IM.Core.Import.BLL.Interface.Import.Models.Settings;
using IM.Core.Import.BLL.Interface.Import.ServiceCatalogue;
using InfraManager;
using InfraManager.DAL;
using InfraManager.DAL.Import.ServiceCatalogue;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.ServiceBase.ImportService.WebAPIModels.ServiceCatalogue;
using InfraManager.ServiceBase.ScheduleService;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.RegularExpressions;

namespace IM.Core.Import.BLL.Import.ServiceCatalogue
{
    public class SCImportModelModificator : ISCImportModelModificator, ISelfRegisteredService<ISCImportModelModificator>
    {
        private readonly ILogger<SCImportModelModificator> _logger;
        private readonly IReadonlyRepository<ServiceCatalogueImportCSVConfiguration> _sciCSVConfigurationReadonlyRepository;
        private readonly IReadonlyRepository<ServiceCatalogueImportCSVConfigurationConcordance> _sciCSVConfigurationConcordanceReadonlyRepository;
        private readonly IScriptDataParser<ConcordanceSCObjectType> _interpretator;

        public SCImportModelModificator(ILogger<SCImportModelModificator> logger,
            IReadonlyRepository<ServiceCatalogueImportCSVConfiguration> sciCSVConfigurationReadonlyRepository,
            IReadonlyRepository<ServiceCatalogueImportCSVConfigurationConcordance> sciCSVConfigurationConcordanceReadonlyRepository,
            IScriptDataParser<ConcordanceSCObjectType> interpretator)
        {
            _logger = logger;
            _sciCSVConfigurationReadonlyRepository = sciCSVConfigurationReadonlyRepository;
            _sciCSVConfigurationConcordanceReadonlyRepository = sciCSVConfigurationConcordanceReadonlyRepository;
            _interpretator = interpretator;
        }
        public async Task<SCImportDetail[]> GetModelsAsync(IProtocolLogger protocolLogger, ImportTaskRequest importTasksDetails, ServiceCatalogueImportSettingData settings, CancellationToken cancellationToken)
        {
            protocolLogger.Information("Подключен источник данных: CSV");
            protocolLogger.AddInputData(InfraManager.ServiceBase.ImportService.Log.ImportInputType.CSV, path: settings.Path);

            ServiceCatalogueImportCSVConfiguration? configuration = null;
            try
            {
                configuration = await _sciCSVConfigurationReadonlyRepository.FirstOrDefaultAsync(x => x.ID == settings.ServiceCatalogueImportCSVConfigurationID, cancellationToken);
            }
            catch (Exception e)
            {
                protocolLogger.Error(e, $"Error when getting Configuration for import service catalogue with id = {importTasksDetails.SettingID}");
                throw;
            }
            string[]? csvLines = null;
            try
            {
                csvLines = GetLinesFromCSV(settings.Path).ToArray();

                protocolLogger.Information($"Прочитано {csvLines.Length - 1} строк(и) данных");
            }
            catch (Exception e)
            {
                protocolLogger.Error(e, $"Error when read file {settings.Path}");
                protocolLogger.Information($"ERR Ошибка чтения файла {settings.Path}");
                throw;
            }

            var fields = _sciCSVConfigurationConcordanceReadonlyRepository.Where(x => x.ServiceCatalogueImportCSVConfigurationID == configuration.ID).ToArray();
            return GetModelsFromCSV(csvLines, fields, configuration.Delimeter, protocolLogger);
        }

        private SCImportDetail[] GetModelsFromCSV(string[] csvLines, ServiceCatalogueImportCSVConfigurationConcordance[] fields, string delimeter, IProtocolLogger protocolLogger)
        {
            ScriptDataDetails<ConcordanceSCObjectType>[] importDetailsArray = new ScriptDataDetails<ConcordanceSCObjectType>[fields.Length];
            for (int i = 0; i < fields.Length; i++)
            {
                importDetailsArray[i] = new ScriptDataDetails<ConcordanceSCObjectType> { FieldEnum = GetEnumByName(fields[i].Field), Script = fields[i].Expression };
            }
            var fieldNames = csvLines[0].Split(delimeter);

            SCImportDetail[] importModels = new SCImportDetail[csvLines.Length - 1];
            var scripts = _interpretator.GetParserData(importDetailsArray);
            for (int i = 1; i < csvLines.Length; i++)
            {
                string[]? data;
                if (IsContainsQuotes(csvLines[i]))
                {
                    Dictionary<string, string> breadcrumbs = GetAllBreadcrumbsInLine(csvLines[i]);
                    csvLines[i] = ChangeValueToBreadcrumbs(csvLines[i], breadcrumbs);
                    data = csvLines[i].Split(delimeter);
                    ChangeBreadcrumbsToValue(data, breadcrumbs);
                }
                else
                {
                    data = csvLines[i].Split(delimeter);
                }
                var resultInterpretator = _interpretator.ParseToDictionary(scripts, fieldNames, data);
                importModels[i - 1] = ImportHelper.GetModelForSC(resultInterpretator);
            }
            protocolLogger.Information($"Загружено {importModels.Length} запись(ей)");
            return importModels;
        }

        private void ChangeBreadcrumbsToValue(string[] data, Dictionary<string, string> breadcrumbs)
        {
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = breadcrumbs[data[i]].Replace("\"", "");
            }
        }

        private string ChangeValueToBreadcrumbs(string parseString, Dictionary<string, string> breadcrumbs)
        {
            foreach (var breadcrumb in breadcrumbs)
            {
                var start = parseString.IndexOf(breadcrumb.Value);
                var end = breadcrumb.Value.Length;
                parseString = parseString.Insert(start + end, breadcrumb.Key);
                parseString = parseString.Remove(start, end);
            }

            return parseString;
        }

        private Dictionary<string, string> GetAllBreadcrumbsInLine(string parseString)
        {
            Dictionary<string, string> breadcrumbs = new Dictionary<string, string>();

            int crumbCount = 0;
            foreach (Match match in Regex.Matches(parseString, "\"([^\"]*)\""))
            {
                var breadcrumb = $"breadcrumb{crumbCount}";
                breadcrumbs.Add(breadcrumb, match.ToString());
                crumbCount++;
            }

            return breadcrumbs;
        }

        private bool IsContainsQuotes(string parseString)
        {
            return parseString.Contains("\"");
        }

        private ConcordanceSCObjectType GetEnumByName(string field)
        {
            return (ConcordanceSCObjectType)System.Enum.Parse(typeof(ConcordanceSCObjectType), field);
        }

        private IEnumerable<string> GetLinesFromCSV(string fileName)
        {
            var path = Path.Combine(Environment.GetEnvironmentVariable("CSVFILESDIR"), fileName);
            using (var reader = File.OpenRead(path))
            {
                using StreamReader streamReader = new StreamReader(reader, CodePagesEncodingProvider.Instance.GetEncoding(1251));
                while (!streamReader.EndOfStream)
                {
                    yield return streamReader.ReadLine();
                }
            }

        }
    }
}
