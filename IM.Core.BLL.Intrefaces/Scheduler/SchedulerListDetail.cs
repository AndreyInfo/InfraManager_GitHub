using InfraManager.ServiceBase.ScheduleService;
using InfraManager.Services.ScheduleService;
using System;

namespace InfraManager.BLL.Scheduler
{
    public class SchedulerListDetail
    {
        public Guid ID { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public bool Schedule { get; init; }
        public TaskState TaskState { get; init; }
        public DateTime? NextRunAt { get; init; }
        public DateTime? FinishRunAt { get; init; }
        public string TaskStateName { get; init; }
        public TaskType TaskType { get; init; }
    }
}
