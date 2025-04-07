using System;
using System.Collections.Generic;
using System.Text;

namespace InfraManager.CrossPlatform.WebApi.Contracts.Scheduler
{
    /// <summary>
    /// Задача по расписанию
    /// </summary>
    public class ScheduleTask : TaskRunInfo
    {
        public Guid TaskID { get; set; }
        public Guid? SettingID { get; set; }
    }
}
