using System.Text;
using AutoMapper;
using IM.Core.Import.BLL.Interface.Import.Models;
using IM.Core.Import.BLL.Interface.Import.Models.Settings;
using IM.Core.Import.BLL.Interface.Import.OSU;
using InfraManager;
using InfraManager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.Import;
using InfraManager.DAL.Import.CSV;
using InfraManager.ServiceBase.ScheduleService;
using IM.Core.Import.BLL.Interface.Configurations;
using Microsoft.Extensions.Logging;
using IM.Core.Import.BLL.Interface;
using IM.Core.Import.BLL.Interface.Import.Log;

namespace IM.Core.Import.BLL.Import.OSU
{
    public class ImportCSVBLL : ImportDeleteSettings<UICSVSetting>, IImportCSVBLL, ISelfRegisteredService<IImportCSVBLL>
    {
        private readonly IReadonlyRepository<UICSVConfiguration> _readOnlyUICSVConfigurationRepository;
        private readonly IReadonlyRepository<UICSVSetting> _readOnlyUICSVSettingRepository;
        private readonly IReadonlyRepository<UICSVIMFieldConcordance> _readOnlyUICSVIMFieldConcordanceRepository;
        private readonly IConfigurationCSVBLL _configurationCSVBLL;
        private readonly ILocalLogger<ImportCSVBLL> _logger;
        private readonly IScriptDataParser<ConcordanceObjectType> _interpretator;

        private readonly IRepository<UICSVSetting> _uiCSVSettingRepository;

        private readonly IUnitOfWork _saveChangesCommand;

        private readonly IMapper _mapper;
        public ImportCSVBLL(
            IReadonlyRepository<UICSVConfiguration> readOnlyUICSVConfigurationRepository,
            IReadonlyRepository<UICSVSetting> readOnlyUICSVSettingRepository,
            IReadonlyRepository<UICSVIMFieldConcordance> readOnlyUICSVIMFieldConcordanceRepository,
            IRepository<UICSVSetting> uiCSVSettingRepository,
            IUnitOfWork saveChangesCommand,
            IScriptDataParser<ConcordanceObjectType> interpretator,
            ILocalLogger<ImportCSVBLL> logger,
            IMapper mapper):base(uiCSVSettingRepository)
        {
            _readOnlyUICSVConfigurationRepository = readOnlyUICSVConfigurationRepository;
            _readOnlyUICSVSettingRepository = readOnlyUICSVSettingRepository;
            _readOnlyUICSVIMFieldConcordanceRepository = readOnlyUICSVIMFieldConcordanceRepository;
            _uiCSVSettingRepository = uiCSVSettingRepository;
            _saveChangesCommand = saveChangesCommand;
            _interpretator = interpretator;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<ImportModel?[]> GetImportModelsAsync(ImportTaskRequest importDetails, UISetting settings,
            IProtocolLogger protocolLogger,
            Func<UISetting?, IProtocolLogger, CancellationToken, Task> verify,
            CancellationToken cancellationToken)
        {
            _logger.Information("Подключен источник данных: CSV");

            UICSVSetting? uiCSVSettings = null;
            try
            {
                uiCSVSettings = await _readOnlyUICSVSettingRepository.FirstOrDefaultAsync(x => x.ID == settings.ID, cancellationToken);
                protocolLogger.AddInputData(InfraManager.ServiceBase.ImportService.Log.ImportInputType.CSV, path: uiCSVSettings.Path);
            }
            catch (Exception e)
            {
                _logger.Error(e, $"Error when getting UICSVSetting with id = {settings.ID}");
                throw;
            }

            await verify(settings, protocolLogger, cancellationToken);
            
            string[]? csvLines;
            try
            {
                csvLines = GetLinesFromCSV(uiCSVSettings.Path).ToArray();

                _logger.Information($"Прочитано {csvLines.Count() - 1} строк(и) данных");
            }
            catch (Exception e)
            {
                _logger.Error(e, $"Ошибка чтения файла {uiCSVSettings.Path}");
                throw;
            }

            var fieldsByConfigurationID = await GetFieldsByConfigurationIDAsync(uiCSVSettings.CSVConfigurationID, cancellationToken);
            var fields = fieldsByConfigurationID.Cast<UICSVIMFieldConcordance>().ToArray();

            return GetImportClassFromCSV(csvLines, fields, uiCSVSettings.Configuration.Delimiter);
        }


        public async Task<Guid?> GetConfigurationIDBySettingAsync(Guid settingID, CancellationToken cancellationToken)
        {
            UICSVSetting? uiCSVSettings = null;
            try
            {
                uiCSVSettings = await _readOnlyUICSVSettingRepository.FirstOrDefaultAsync(x => x.ID == settingID, cancellationToken);
            }
            catch (Exception e)
            {
                _logger.Error(e, $"Error when getting UICSVSetting with id = {settingID}");
                throw;
            }

            return uiCSVSettings.CSVConfigurationID;
        }
        public async Task<IEnumerable<UIIMFieldConcordance>> GetFieldsByConfigurationIDAsync(Guid? configurationID,
            CancellationToken token)
        {
            return await _readOnlyUICSVIMFieldConcordanceRepository.ToArrayAsync(x => x.CSVConfigurationID == configurationID);
        }
        public async Task<CSVConfigurationTable[]> GetConfigurationTableAsync(CancellationToken cancellationToken)
        {
            var table = await _readOnlyUICSVConfigurationRepository.ToArrayAsync(cancellationToken);
            return _mapper.Map<CSVConfigurationTable[]>(table);
        }

        public async Task SetUISettingAsync(Guid idUISetting, Guid? idCSVConfiguration, CancellationToken cancellationToken)
        {
            var uiCsvSetting = new UICSVSetting(idUISetting, idCSVConfiguration);
            _uiCSVSettingRepository.Insert(uiCsvSetting);

            await _saveChangesCommand.SaveAsync(cancellationToken);

        }
        public async Task<Guid?> GetIDConfiguration(Guid id, CancellationToken cancellationToken)
        {
            var setting = await _readOnlyUICSVSettingRepository.FirstOrDefaultAsync(x => x.ID == id, cancellationToken);
            return (setting.Configuration != null) 
                ? setting?.Configuration.ID
                : null;
        }

        public async Task UpdateUISettingAsync(Guid id, Guid? selectedConfiguration, CancellationToken cancellationToken)
        {
            var uiCsvSetting = await _uiCSVSettingRepository.FirstOrDefaultAsync(x => x.ID == id, cancellationToken);
            if (uiCsvSetting is not null)
            {
                uiCsvSetting.CSVConfigurationID = selectedConfiguration;
                await _saveChangesCommand.SaveAsync(cancellationToken);
            }
        }

        public async Task<string> GetPathAsync(Guid id, CancellationToken cancellationToken)
        {
            var setting = await _readOnlyUICSVSettingRepository.FirstOrDefaultAsync(x => x.ID == id, cancellationToken);
            return setting!=null ? $"\"{setting.Path}\"":String.Empty;
        }

        public async Task UpdatePathAsync(Guid id, string path, CancellationToken cancellationToken)
        {
            var setting = await _uiCSVSettingRepository.FirstOrDefaultAsync(x => x.ID == id, cancellationToken);
            setting.Path = path;

            await _saveChangesCommand.SaveAsync(cancellationToken);
        }

        private ImportModel[] GetImportClassFromCSV(string[] csvLines, UICSVIMFieldConcordance[] fields, string delimeter)
        {
            ScriptDataDetails<ConcordanceObjectType>[] importDetailsArray = new ScriptDataDetails<ConcordanceObjectType>[fields.Length];
            for (int i = 0; i < fields.Count(); i++)
            {
                importDetailsArray[i] = new ScriptDataDetails<ConcordanceObjectType> { FieldEnum = (ConcordanceObjectType)fields[i].IMFieldID, Script = fields[i].Expression };
            }
            var fieldNames = csvLines[0].Split(delimeter);

            ImportModel[] importModels = new ImportModel[csvLines.Length - 1];
            var scripts = _interpretator.GetParserData(importDetailsArray);

            for (int i = 1; i < csvLines.Count(); i++)
            {
                var data = csvLines[i].Split(delimeter);
                var resultInterpretator = _interpretator.ParseToObjectDictionary(scripts, fieldNames, data);
                importModels[i - 1] = ImportHelper.GetImportModel(resultInterpretator);
            }

            return importModels;
        }

        private IEnumerable<string?> GetLinesFromCSV(string fileName)
        {
            var csvFileDirectory = Environment.GetEnvironmentVariable("CSVFILESDIR")
                ?? throw new ObjectNotFoundException("Не удалось извлечь конфигурацию для CSVFILEDIR");
            var path = Path.Combine(csvFileDirectory, fileName);

            using var reader = File.OpenRead(path);
            
            var encoding = CodePagesEncodingProvider.Instance.GetEncoding(1251)
                           ?? throw new ObjectNotFoundException("Не найдена кодировка для кодовой страницы 1251");
            using var streamReader = new StreamReader(reader, encoding);
            while (!streamReader.EndOfStream)
            {
                yield return streamReader.ReadLine();
            }
        }


    }
}
