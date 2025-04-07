using InfraManager.CrossPlatform.WebApi.Contracts.Scheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.CrossPlatform.BLL.Intrefaces.Schedule
{
    public interface IScheduleBLL
    {
        Task<List<ScheduleTask>> GetTasks();
    }
}
