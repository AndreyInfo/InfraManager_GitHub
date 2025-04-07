using Inframanager;
using System;
using InfraManager.DAL.FormBuilder;
using InfraManager.DAL.ServiceCatalogue;

namespace InfraManager.DAL.ServiceDesk.MassIncidents
{
    /// <summary>
    /// Этот класс представляет сущность Тип массового инцидента
    /// </summary>
    [ObjectClassMapping(ObjectClass.MassIncidentType)]
    [OperationIdMapping(ObjectAction.ViewDetails, OperationID.MassIncidentType_Properties)]
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.MassIncidentType_Properties)]
    [OperationIdMapping(ObjectAction.Insert, OperationID.MassIncidentType_Add)]
    [OperationIdMapping(ObjectAction.Update, OperationID.MassIncidentType_Update)]
    [OperationIdMapping(ObjectAction.Delete, OperationID.MassIncidentType_Delete)]
    public class MassIncidentType : IMarkableForDelete, IGloballyIdentifiedEntity, IFormBuilder
    {
        protected MassIncidentType()
        {
        }

        /// <summary>
        /// Создает новый тип массового инцидента
        /// </summary>
        /// <param name="name"></param>
        public MassIncidentType(string name)
        {
            Name = name;
            Removed = false;
        }

        /// <summary>
        /// Возвращает идентификатор
        /// </summary>
        public int ID { get; }

        /// <summary>
        /// Возвращает глобальный идентификатор
        /// </summary>
        public Guid IMObjID { get; }
        
        /// <summary>
        /// Возвращает или задает название
        /// </summary>
        public string Name { get; init; }
        
        /// <summary>
        /// Возвращает или задает идентификатор схемы рабочей процедуры
        /// </summary>
        public string WorkflowSchemeIdentifier { get; init; }
        
        /// <summary>
        /// Возвращает или задает идентификатор Формы
        /// </summary>
        public Guid? FormID { get; init; }
        
        /// <summary>
        /// Возвращает или задает идентификатор версии
        /// </summary>
        public byte[] RowVersion { get; init; }
        
        /// <summary>
        /// Возвращает признак удаления
        /// </summary>
        public bool Removed { get; private set; }
        
        /// <summary>
        /// Помечает тип массового инцидента на удаление
        /// </summary>
        public void MarkForDelete()
        {
            Removed = true;
        }

        /// <summary>
        /// Возвращает шаблон формы доп. параметров для этого типа массового инцидента.
        /// </summary>
        public virtual Form Form { get; }
    }
}
