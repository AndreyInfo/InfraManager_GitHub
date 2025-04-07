
using InfraManager.DAL.Import;
using InfraManager.Services.ScheduleService;

namespace IM.Core.Import.BLL.Interface
{
    public interface IImportLogger
    {
        List<TitleLog> GetAllTitleLogsByTaskId(Guid id);
        LogTask GetLogById(Guid id);
        SchedulerProtocolsDetail[] GetAllTitleLogs(SchedulerProtocolsDetail[] tasks);
    }
}
