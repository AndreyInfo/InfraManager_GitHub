using IM.Core.Import.BLL.Interface;
using InfraManager;
using InfraManager.Core.Helpers;
using InfraManager.DAL.Import;
using InfraManager.ServiceBase.ImportService.Log;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;
using Serilog.Core;
using System.Globalization;

namespace IM.Core.Import.BLL;

internal class ProtocolLogger : IProtocolLogger, ISelfRegisteredService<IProtocolLogger>
{
    private ILogger _logger;
    private ImportTaskTypeEnum _taskType;
    private MainLog _log;
    private readonly LoggerSettingsOptions _options;
    private string _informationLogsPath;
    private string _fullLogsPath;


    public ProtocolLogger(IOptions<LoggerSettingsOptions> options)
    {
        _options = options.Value;
    }
    private void SetSettings(Guid taskID, Guid settingID, ImportTaskTypeEnum taskType, string startDate)
    {
        _taskType = taskType;
        _informationLogsPath = Path.Combine(Environment.GetEnvironmentVariable("LOGDIR"), _options.InformationPath);
        _fullLogsPath = Path.Combine(Environment.GetEnvironmentVariable("LOGDIR"), _options.FullLogsPath);
        string informationFile = _informationLogsPath + TypeHelper.GetFriendlyEnumFieldName(taskType) + "_id_" + taskID + "_tid_" + settingID + "_" + startDate + ".txt";
        string fullLogsFile = _fullLogsPath + TypeHelper.GetFriendlyEnumFieldName(taskType) + "_id_" + taskID + "_tid_" + settingID + "_" + startDate + ".txt";

        _logger = new LoggerConfiguration()
           .MinimumLevel.Debug()
           .Filter.ByIncludingOnly(x => x.Level == Serilog.Events.LogEventLevel.Information || x.Level == Serilog.Events.LogEventLevel.Debug)
           .WriteTo.File(informationFile,
outputTemplate: "{Message}{NewLine}{Exception}")
           .Filter.ByIncludingOnly(x => x.Level == Serilog.Events.LogEventLevel.Information || x.Level == Serilog.Events.LogEventLevel.Debug || x.Level == Serilog.Events.LogEventLevel.Error || x.Level == Serilog.Events.LogEventLevel.Verbose)
           .WriteTo.File(fullLogsFile,
outputTemplate: "{Timestamp:o} [{Level}] ({SourceContext}.{Method}) {Message}{NewLine}{Exception}")
           .CreateLogger();
    }

    public void Debug(string information)
    {
        _logger.Debug(information);
    }

    public void Information(string information)
    {
        _logger.Information(information);
    }

    public void StartTask(string taskName, ImportTaskTypeEnum taskType, Guid taskID, Guid taskScheduleID)
    {
        _log = new MainLog(taskName, taskID, taskScheduleID, _taskType);
        SetSettings(_log.ID, taskID, taskType, _log.StartDate.ToString("MM.dd.yyyy.HH.mm.ss", CultureInfo.InvariantCulture));
    }

    public void AddInputData(ImportInputType type, string taskNote = "", string path = "")
    {

        ImportInputData importInputData = new ImportInputData(type, path);
        _log.ImportInputData = importInputData;
        Information(JsonConvert.SerializeObject(_log));
    }
   
    public void Verbose(string information)
    {
        _logger.Verbose(information);
    }

    public void Error(Exception exception, string message)
    {
        _logger.Error(exception, message);
    }

    public void CheckCreateValidProtocol()
    {
        if (_log.ImportInputData is null)
        {
            throw new ArgumentException("Не задано устройство ввода для задачи импорта!");
        }
    }
    public void FlushAndClose()
    {
        ((Logger)_logger).Dispose();
    }
}
