using AutoMapper;
using IM.Core.Import.BLL.Interface;
using IM.Import.Services.Logger;
using InfraManager;
using InfraManager.BLL.Import;
using InfraManager.DAL.Import;
using InfraManager.Services.ScheduleService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using DateTime = System.DateTime;

namespace IM.Core.Import.BLL
{
    internal class ImportLogger : IImportLogger, ISelfRegisteredService<IImportLogger>
    {
        private readonly IImportApi _api;
        public ImportLogger(IImportApi api)
        {
            _api = api;
        }

        public async Task<List<TitleLog>> GetAllTitleLogsByTaskIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _api.GetAllTitleLogsByTaskIdAsync(id);
        }

        public async Task<LogTask> GetLogByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _api.GetLogByIdAsync(id);
        }

        
        public async Task<SchedulerProtocolsDetail[]> GetAllTitleLogsAsync(SchedulerProtocolsDetail[] tasks, CancellationToken cancellationToken)
        {
            return await _api.GetAllTitleLogsAsync(tasks, cancellationToken);
        }
    }
}
