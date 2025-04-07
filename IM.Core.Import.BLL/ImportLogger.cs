using AutoMapper;
using IM.Core.Import.BLL.Interface;
using InfraManager;
using InfraManager.DAL.Import;
using InfraManager.ServiceBase.ImportService.Log;
using InfraManager.Services.ScheduleService;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Globalization;
using System.Text.RegularExpressions;
using DateTime = System.DateTime;
using File = System.IO.File;

namespace IM.Core.Import.BLL
{
    internal class ImportLogger : IImportLogger, ISelfRegisteredService<IImportLogger>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<ImportLogger> _logger;
        private const int PROTOCOL_END_STRING_LENGTH = 19;
        public ImportLogger(IMapper mapper,
            ILogger<ImportLogger> logger)
        {
            _mapper = mapper;
            _logger = logger;
        }

        public List<TitleLog> GetAllTitleLogsByTaskId(Guid id)
        {
            var logs = GetAllTitleLogs();
            if (logs != null)
            {
                return logs.Where(x => x.TaskId == id).ToList();
            }
            else
            {
                return new List<TitleLog>();
            }
        }

        public SchedulerProtocolsDetail[] GetAllTitleLogs(SchedulerProtocolsDetail[] tasks)
        {
            var titleLogs = GetAllTitleLogs();
            foreach (var task in tasks)
            {
                task.ProtocolIDs = titleLogs.Where(x => x.TaskId == task.TaskSettingID)?.Select(p => p.Id)?.ToArray();
            }
            return tasks;
        }

        public LogTask GetLogById(Guid id)
        {
            string path = Path.Combine(Environment.GetEnvironmentVariable("LOGDIR"), "logs", "Information");
            if (Directory.Exists(path))
            {
                var files = Directory.GetFiles(path);

                if (files.Length > 0)
                {
                    foreach (var file in files)
                    {
                        var fileName = Path.GetFileName(file);
                        var idFromFile = Regex.Match(fileName, @"_id_(.*?)_").Groups[1].ToString();
                        if (id.Equals(GetGuid(idFromFile)))
                        {
                            return GetLogTasks(path, fileName);
                        }
                    }
                }
            }
            throw new ArgumentException($"Нет директории {path}!");
        }

        private IEnumerable<TitleLog> GetAllTitleLogs()
        {
            string path = Path.Combine(Environment.GetEnvironmentVariable("LOGDIR"), "logs", "Information");
            if (Directory.Exists(path))
            {
                var files = Directory.GetFiles(path);

                if (files.Count() > 0)
                {
                    foreach (var file in files)
                    {
                        var fileName = Path.GetFileName(file);
                        var type = Regex.Match(fileName, @"(.*?)_id").Groups[1].ToString();
                        var id = Regex.Match(fileName, @"_id_(.*?)_").Groups[1].ToString();
                        var taskId = Regex.Match(fileName, @"_tid_(.*?)_").Groups[1].ToString();
                        var date = Regex.Match(fileName, @"_(\d{2}\.\d{2}\.\d{4}\.\d{2}\.\d{2}.\d{2})").Groups[1].ToString();

                        yield return new TitleLog(GetGuid(id), GetGuid(taskId), GetDateTime(date));
                    }
                }
            }
        }

        private Guid GetGuid(string id)
        {
            if (Guid.TryParse(id, out var guid))
            {
                return guid;
            }
            else
            {
                throw new ArgumentException($"Не удается распарсить GUID:{id}");
            }
        }

        private LogTask GetLogTasks(string path, string fileName)
        {
            string fullPath = Path.Combine(path, fileName);
            string log;
            using (var stream = File.Open(fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var reader = new StreamReader(stream))
            {
                log = reader.ReadToEnd();
            }

            var startIndexLog = log.IndexOf("{\"TaskID\"");
            var endIndexLog = log.IndexOf("\"End\":\"EndJsonLog\"}");
            var jsonLog = log.Substring(startIndexLog, endIndexLog - startIndexLog + PROTOCOL_END_STRING_LENGTH);
            var mainLog = JsonConvert.DeserializeObject<MainLog>(jsonLog);
            var body = log.Remove(startIndexLog, jsonLog.Length);

            var logTask = _mapper.Map<LogTask>(mainLog);
            logTask.Log = body;
            return logTask;
        }

        private DateTime? GetDateTime(string time)
        {
            if (time == null)
                return null;

            try
            {
                return DateTime.ParseExact(time, "MM.dd.yyyy.HH.mm.ss",
                                  CultureInfo.InvariantCulture);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error parse Datetime from string in logs");
                return null;
            }
        }
    }
}
