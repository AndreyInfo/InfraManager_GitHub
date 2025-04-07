using InfraManager.Services.ScheduleService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.DAL
{
    public class ScheduleEntity
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid ID { get; init; }
        /// <summary>
        /// Начало
        /// </summary>
        public DateTime? StartAt { get; set; }
       
        /// <summary>
        /// Окончание
        /// </summary>
        public DateTime? FinishAt { get; set; }

        /// <summary>
        /// Интервал запуска
        /// </summary>
        public int Interval { get; set; } = 1;
        public ScheduleTypeEnum ScheduleType { get; set; }
        public string DaysOfWeek { get; set; }
        public string Months { get; set; }
        public Guid ScheduleTaskEntityID { get; set; }
        /// <summary>
        /// Следующий запуск
        /// </summary>
        public DateTime? NextAt { get; set; }
        /// <summary>
        /// Последний запуск
        /// </summary>
        public DateTime? LastAt { get; set; }
        public virtual ScheduleTaskEntity ScheduleTask { get;}
    }
}
