using Inframanager;
using System;

namespace InfraManager.DAL.ServiceDesk.MassIncidents
{
    /// <summary>
    /// Этот класс представляет сущность Причина массового инцидента
    /// </summary>
    [ObjectClassMapping(ObjectClass.MassIncidentCause)]
    [OperationIdMapping(ObjectAction.Insert, OperationID.MassIncidentCause_Add)]
    [OperationIdMapping(ObjectAction.Update, OperationID.MassIncidentCause_Update)]
    [OperationIdMapping(ObjectAction.Delete, OperationID.MassIncidentCause_Delete)]
    [OperationIdMapping(ObjectAction.ViewDetails, OperationID.MassIncidentCause_Properties)]
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.MassIncidentCause_Properties)]
    public class MassIncidentCause : IMarkableForDelete
    {
        /// <summary>
        /// Возвращает идентификатор
        /// </summary>
        public int ID { get; }
        /// <summary>
        /// Возвращает глобальный идентификатор
        /// </summary>
        public Guid IMObjID { get; }
        /// <summary>
        /// Возвращает или задает наименование
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Возвращает или задает идентификатор версии
        /// </summary>
        public byte[] RowVersion { get; set; }
        /// <summary>
        /// Возвращает признак удаления
        /// </summary>
        public bool Removed { get; private set; }
        /// <summary>
        /// Помечает причину массового инцидента как удаленую
        /// </summary>
        public void MarkForDelete()
        {
            Removed = true;
        }
    }
}
