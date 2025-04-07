using System;
using System.Collections.Generic;
using Inframanager;

namespace InfraManager.DAL
{
    [ObjectClassMapping(ObjectClass.ScheduleTask)]
    [OperationIdMapping(ObjectAction.Delete, OperationID.ScheduleTask_Delete)]
    [OperationIdMapping(ObjectAction.Insert, OperationID.ScheduleTask_Create)]
    [OperationIdMapping(ObjectAction.Update, OperationID.ScheduleTask_Save)]
    [OperationIdMapping(ObjectAction.ViewDetails, OperationID.ScheduleTask_Open)]
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.ScheduleTask_Open)]
    public class ScheduleTaskEntity
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid ID { get; init; }
        
        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; init; }
        
        /// <summary>
        /// Тип задачи
        /// </summary>
        public TaskTypeEnum TaskType { get; init; }
        
        /// <summary>
        /// Тип задачи
        /// </summary>
        public Guid TaskSettingID { get; init; }
        
        /// <summary>
        /// Название задачи
        /// </summary>
        public string TaskSettingName { get; init; }
        
        /// <summary>
        /// Примечание
        /// </summary>
        public string Note { get; init; }
        
        /// <summary>
        /// Разрешено
        /// </summary>
        public bool IsEnabled { get; init; }
        
        /// <summary>
        /// использовать учетную запись
        /// </summary>
        public bool UseAccount { get; init; }
        
        /// <summary>
        /// Состояние
        /// </summary>
        public TaskStateEnum TaskState { get; set; }
        
        /// <summary>
        /// Ближайший запуск
        /// </summary>
        public DateTime? NextRunAt { get; set; }
        
        /// <summary>
        /// Завершение последнего запуска
        /// </summary>
        public DateTime? FinishRunAt { get; set; }
        
        /// <summary>
        /// идентификатор в таблице с учетными данными
        /// </summary>
        public int? CredentialID { get; set; }
        
        /// <summary>
        /// Старт последнего запуска
        /// </summary>
        public DateTime? LastStartAt { get; set; }
        
        public Guid? CurrentExecutingScheduleID { get; set; }
        
        public virtual ScheduleEntity CurrentSchedule { get; init; }

        public virtual ICollection<ScheduleEntity> Schedules { get; init; }
    }
    
}
