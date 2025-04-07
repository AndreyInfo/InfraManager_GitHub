using InfraManager.CrossPlatform.WebApi.Contracts.Common.Attributes;
using System;

namespace InfraManager.CrossPlatform.WebApi.Contracts.Scheduler
{
    /// <summary>
    /// Базовая модель задания
    /// </summary>
    public class TaskRunInfo
    {
        /// <summary>
        /// Описание расписания
        /// </summary>
        public string ScheduleLabel { get; set; }

        /// <summary>
        /// Представление сотсояния
        /// </summary>
        public string StateLabel { get; set; }

        /// <summary>
        /// Следующий запус
        /// </summary>
        public DateTime? NextRun { get; set; }


        /// <summary>
        /// Полний запуск
        /// </summary>
        public DateTime? LastRun { get; set; }

    }
}
